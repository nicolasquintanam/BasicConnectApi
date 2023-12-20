namespace BasicConnectApi.Services;

using BasicConnectApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string name)
    {
        var tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
        var key = Encoding.ASCII.GetBytes(tokenOptions.Secret);
        var jti = Guid.NewGuid().ToString();


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Name, name)
            }),
            Expires = DateTime.UtcNow.AddDays(tokenOptions.ExpiryDays),
            Audience = tokenOptions.Audience,
            Issuer = tokenOptions.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}