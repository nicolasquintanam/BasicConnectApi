namespace BasicConnectApi.Services;

using BasicConnectApi.Models;
using System.IdentityModel.Tokens.Jwt;
using BasicConnectApi.Enums;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BasicConnectApi.Data;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;
    private readonly IApplicationDbContext _dbContext;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger, IApplicationDbContext dbContext)
    {
        _configuration = configuration;
        _logger = logger;
        _dbContext = dbContext;
    }

    public string GenerateToken(string userId, TokenTypeEnum tokenType = TokenTypeEnum.AccessToken)
    {
        var jwtConfiguration = _configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        if (jwtConfiguration is null || jwtConfiguration.Secret is null)
            return string.Empty;
        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);
        var jti = Guid.NewGuid().ToString();


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("scope", tokenType == TokenTypeEnum.TemporaryToken ? "temporary-token" : "access-token")
            }),
            Expires = tokenType == TokenTypeEnum.TemporaryToken ? DateTime.UtcNow.AddMinutes(15) : DateTime.UtcNow.AddDays(jwtConfiguration.ExpiryDays),
            Audience = jwtConfiguration.Audience,
            Issuer = jwtConfiguration.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var result = tokenHandler.WriteToken(token);

        return result;
    }

    public void RevokeToken(string token)
    {
        int? userId = GetUserIdFromToken(token);
        if (userId is null)
            return;
        var tokenId = GetJtiFromToken(token);
        if (tokenId is null)
            return;
        var revokedToken = new RevokedToken
        {
            TokenId = tokenId,
            UserId = userId.Value
        };

        _dbContext.RevokedToken.Add(revokedToken);
        _dbContext.SaveChanges();
    }

    public bool TokenIsRevoked(string tokenId)
    {
        var revoked = _dbContext.RevokedToken.FirstOrDefault(u => u.TokenId == tokenId);

        return revoked is not null;
    }

    public string? GetTokenFromAuthorizationHeader(IHeaderDictionary headers)
    {
        if (headers.TryGetValue("Authorization", out var authorizationHeader) &&
            !string.IsNullOrWhiteSpace(authorizationHeader) &&
            authorizationHeader.ToString().StartsWith("Bearer "))
        {
            return authorizationHeader.ToString().Substring("Bearer ".Length);
        }

        return null;
    }

    public string? GetJtiFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var jti = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "jti")?.ToString()["jti: ".Length..];

        return jti;
    }

    public int? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid");

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    public bool ValidateTemporaryToken(string token)
    {
        var jwtConfiguration = _configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        if (jwtConfiguration is null || jwtConfiguration.Secret is null)
            return false;
        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Secret)),
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (!principal.HasClaim(claim => claim.Type == "scope" && claim.Value == "temporary-token"))
                return false;

            string? tokenId = GetJtiFromToken(token);
            if (tokenId is null)
                return false;
            if (TokenIsRevoked(tokenId))
                return false;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reset Password Token isn't valid");
            return false;
        }
    }

    public bool ValidateAccessToken(string token)
    {
        var jwtConfiguration = _configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        if (jwtConfiguration is null || jwtConfiguration.Secret is null)
            return false;
        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Secret)),
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (!principal.HasClaim(claim => claim.Type == "scope" && claim.Value == "access-token"))
                return false;

            string? tokenId = GetJtiFromToken(token);
            if (tokenId is null)
                return false;
            if (TokenIsRevoked(tokenId))
                return false;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Access Token isn't valid");
            return false;
        }
    }
}