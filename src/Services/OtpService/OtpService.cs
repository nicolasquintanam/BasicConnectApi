using BasicConnectApi.Data;
using BasicConnectApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicConnectApi.Services;

public class OtpService(IApplicationDbContext dbContext, IUserService userService, ITokenService tokenService, IHashService hashService) : IOtpService
{
    public enum ContextEnum
    {
        password_recovery,
        confirm_email
    }
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IHashService _hashService = hashService;
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<string> GenerateOtp(string email, string context)
    {
        if (!await _userService.ExistsUser(email))
            return string.Empty;
        if (!ExistsContext(context))
            return string.Empty;
        var otpValue = _tokenService.GenerateToken(5);
        int? userId = await _userService.GetUserId(email);
        OneTimePassword otp = new()
        {
            Context = context,
            ExpiryTime = DateTime.UtcNow.AddMinutes(5),
            OtpValue = _hashService.HashData(otpValue),
            UserId = userId.Value,
        };
        _dbContext.OneTimePassword.Add(otp);
        _dbContext.SaveChanges();
        return otpValue;
    }
    private static bool ExistsContext(string context) =>
        Enum.TryParse<ContextEnum>(context, true, out _);

    public async Task<bool> ValidateOtp(string email, string otp, string context, bool validateUsed = true)
    {
        if (!await _userService.ExistsUser(email))
            return false;
        var userId = await _userService.GetUserId(email);
        if (!ExistsContext(context))
            return false;

        var otps = _dbContext.OneTimePassword
            .Where(a => a.UserId == userId.Value)
            .Where(a => a.Context == context)
            .Where(a => a.OtpValue == _hashService.HashData(otp))
            .Where(a => a.ExpiryTime > DateTime.UtcNow);
        if (validateUsed)
            otps = otps.Where(a => !a.IsUsed);

        var onetimePassword = await otps.FirstOrDefaultAsync();
        if (onetimePassword is null)
            return false;
        onetimePassword.IsUsed = true;
        _dbContext.SaveChanges();

        return true;
    }
}