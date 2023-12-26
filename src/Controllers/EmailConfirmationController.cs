namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IEmailConfirmationService _emailConfirmationService;

    public EmailConfirmationController(IUserService userService, IEmailConfirmationService emailConfirmationService)
    {
        _userService = userService;
        _emailConfirmationService = emailConfirmationService;
    }

    [HttpGet("confirm")]
    public IActionResult ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        var isEmailConfirmed = _emailConfirmationService.ConfirmEmail(request.Email, request.Token);

        if (!isEmailConfirmed)
            return BadRequest(new BaseResponse(false));

        return Ok(new BaseResponse(true));
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] SendConfirmationEmailRequest request)
    {
        await _emailConfirmationService.SendConfirmationEmailAsync(request.Email);
        return Ok(new BaseResponse(true));
    }
}
