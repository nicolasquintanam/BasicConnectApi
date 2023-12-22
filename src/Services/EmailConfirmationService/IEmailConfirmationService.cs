namespace BasicConnectApi.Services;

public interface IEmailConfirmationService
{
    void SendConfirmationEmail(string userEmail);
    bool ConfirmEmail(string userEmail, string confirmationToken);
}
