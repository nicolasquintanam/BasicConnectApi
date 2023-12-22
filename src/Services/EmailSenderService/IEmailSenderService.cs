namespace BasicConnectApi.Services;

public interface IEmailSenderService
{
    void SendConfirmationEmail(string email, string confirmationToken);
}