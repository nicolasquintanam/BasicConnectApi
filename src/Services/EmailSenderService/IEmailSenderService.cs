namespace BasicConnectApi.Services;

public interface IEmailSenderService
{
    Task SendToConfirmEmail(string? email, string? otp);
    Task SendToRecoverPassword(string? email, string? otp);
}