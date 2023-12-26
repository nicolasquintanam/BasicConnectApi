namespace BasicConnectApi.Services;

using BasicConnectApi.Models;
using System.IdentityModel.Tokens.Jwt;
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

    public string GenerateToken(string userId)
    {
        var jwtConfiguration = _configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);
        var jti = Guid.NewGuid().ToString();


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
            Expires = DateTime.UtcNow.AddDays(jwtConfiguration.ExpiryDays),
            Audience = jwtConfiguration.Audience,
            Issuer = jwtConfiguration.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public void RevokeToken(string token)
    {
        int? userId = GetUserIdFromToken(token);
        if (userId is null)
            return;
        var revokedToken = new RevokedToken
        {
            TokenId = GetJtiFromToken(token),
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

        var jti = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "jti").ToString().Substring("jti: ".Length);

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
}