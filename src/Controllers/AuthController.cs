namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using BasicConnectApi.Filters;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ValidationFilter))]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IUserService userService, IConfiguration configuration, IJwtService jwtService)
    {
        _logger = logger;
        _userService = userService;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        //TODO; validate login
        var token = _jwtService.GenerateToken(request.Email);
        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        //TODO; revoke jwt
        return Ok(new { Message = "Session successfully closed" });
    }
}