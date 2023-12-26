namespace BasicConnectApi.Services;

using BasicConnectApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BasicConnectApi.Data;
using BasicConnectApi.Helpers;

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
        int? userId = JwtHelper.GetUserId(token);
        if (userId is null)
            return;
        var revokedToken = new RevokedToken
        {
            TokenId = JwtHelper.GetJti(token),
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
}