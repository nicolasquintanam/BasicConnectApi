namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using BasicConnectApi.Custom;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

[ApiController]
[Route("api/v1/users")]
public class UserController(IUserService userService, IJwtService jwtService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existsUser = await _userService.ExistsUser(request.Email);
        if (existsUser)
            return Conflict(new BaseResponse(false, "The email is already registered"));
        int? userId = _userService.RegisterUser(request.FirstName, request.LastName, request.Email, request.Password);
        if (userId == null)
            return BadRequest(new BaseResponse(false));
        return Ok(new BaseResponse(true) { Data = new { user_id = userId } });
    }

    [HttpGet("{id}")]
    [AccessTokenAuthorize]
    public async Task<IActionResult> GetUserById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new BaseResponse(false, "Invalid 'id' format. 'id' must be an integer or 'me'."));
        var token = _jwtService.GetTokenFromAuthorizationHeader(HttpContext.Request.Headers);
        if (token is null)
            return Unauthorized(new BaseResponse(false));
        var userId = _jwtService.GetUserIdFromToken(token);
        if (userId is null)
            return Unauthorized(new BaseResponse(false));

        if (id != "me" && !id.Equals(userId.ToString()))
            return StatusCode((int)HttpStatusCode.Forbidden, new BaseResponse(false, "You are not authorized to access information for other users."));
        var user = await _userService.GetUserById(userId.Value);
        if (user == null)
            return NotFound(new BaseResponse(false));
        return Ok(new BaseResponse(true) { Data = user });
    }
}
