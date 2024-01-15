namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using BasicConnectApi.Filters;
using BasicConnectApi.Custom;

[ApiController]
[Route("api/v1")]
[ServiceFilter(typeof(ValidationFilter))]
public class AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!_userService.AuthenticateUser(request.Email, request.Password, out int userId))
            return Unauthorized(new BaseResponse(false, "Invalid username or password"));

        var token = _jwtService.GenerateToken(userId.ToString());
        return Ok(new BaseResponse(true) { Data = new { token } });
    }

    [AccessTokenAuthorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var token = _jwtService.GetTokenFromAuthorizationHeader(HttpContext.Request.Headers);
        if (token is not null)
            _jwtService.RevokeToken(token);

        return Ok(new BaseResponse(true));
    }
}
