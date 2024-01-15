namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using BasicConnectApi.Models;
using BasicConnectApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class OtpController(IOtpService otpService, IEmailSenderService emailSenderService, ILogger<OtpController> logger) : ControllerBase
{
    private readonly IOtpService _otpService = otpService;
    private readonly ILogger<OtpController> _logger = logger;
    private readonly IEmailSenderService _emailSenderService = emailSenderService;

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
    {
        string otp = await _otpService.GenerateOtp(request.Email, request.Context);
        if (string.IsNullOrEmpty(otp))
            return Ok(new BaseResponse(true));
        if (request.Context == "confirm_email")
            await _emailSenderService.SendToConfirmEmail(request.Email, otp);
        if (request.Context == "password_recovery")
            await _emailSenderService.SendToRecoverPassword(request.Email, otp);

        return Ok(new BaseResponse(true));
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpRequest request)
    {
        var result = await _otpService.ValidateOtp(request.Email, request.OtpValue, request.Context);
        if (!result)
            return Unauthorized(new BaseResponse(false, "Invalid OTP value."));

        var temporaryToken = await _otpService.GenerateTemporaryAccessToken(request.Email, request.Context);
        if (!string.IsNullOrEmpty(temporaryToken))
            return Ok(new BaseResponse(true) { Data = new { temporary_access_token = temporaryToken } });
        return Ok(new BaseResponse(true));
    }
}
