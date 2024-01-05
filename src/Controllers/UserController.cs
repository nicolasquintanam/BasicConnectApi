namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existsUser = await _userService.ExistsUser(request.Email);
        if (existsUser)
            return Conflict(new BaseResponse(false, "The email is already registered"));
        var userId = _userService.RegisterUser(request.FirstName, request.LastName, request.Email, request.Password);
        return Ok(new BaseResponse(true) { Data = new { user_id = userId } });
    }
}
