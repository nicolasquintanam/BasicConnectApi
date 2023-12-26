namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var existsUser = _userService.ExistsUser(request.Email);
        if (existsUser)
            return Conflict(new BaseResponse(false, "The email is already registered"));
        var userId = _userService.RegisterUser(request.FirstName, request.LastName, request.Email, request.Password);
        return Ok(new BaseResponse(true) { Data = new { user_id = userId } });
    }
}
