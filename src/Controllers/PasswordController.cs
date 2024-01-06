using BasicConnectApi.Models;
using BasicConnectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicConnectApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PasswordController(ILogger<PasswordController> logger, IUserService userService, IJwtService jwtService) : ControllerBase
{
    private readonly ILogger<PasswordController> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;
    [Authorize]
    [HttpPut("reset")]
    public async Task<ActionResult> ResetPassword([FromBody] PasswordResetRequest request)
    {
        var token = _jwtService.GetTokenFromAuthorizationHeader(HttpContext.Request.Headers);
        if (token is null)
            return Unauthorized();
        int? userId = _jwtService.GetUserIdFromToken(token);
        if (userId is null)
            return Unauthorized();
        await _userService.ResetPassword(userId.Value, request.Password);
        _jwtService.RevokeToken(token);
        return Ok(new BaseResponse(true));
    }
}