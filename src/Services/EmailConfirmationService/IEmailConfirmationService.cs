namespace BasicConnectApi.Services;

public interface IEmailConfirmationService
{
    Task SendConfirmationEmailAsync(string userEmail);
    bool ConfirmEmail(string userEmail, string confirmationToken);
}
