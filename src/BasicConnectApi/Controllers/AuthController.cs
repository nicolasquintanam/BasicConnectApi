namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using BasicConnectApi.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;


[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ValidationFilter))]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IUserService userService, IConfiguration configuration)
    {
        _logger = logger;
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
        var key = Encoding.ASCII.GetBytes(tokenOptions.Secret);
        var jti = Guid.NewGuid().ToString();


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Name, request.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(tokenOptions.ExpiryDays),
            Audience = tokenOptions.Audience,
            Issuer = tokenOptions.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { token = tokenHandler.WriteToken(token) });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Session successfully closed" });
    }
}