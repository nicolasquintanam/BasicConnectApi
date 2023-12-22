namespace BasicConnectApi.Services;

public class EmailConfirmationService : IEmailConfirmationService
{
    public async Task SendConfirmationEmailAsync(string userEmail)
    {
        //TODO: implement email confirmation sending
    }

    public bool ConfirmEmail(string userEmail, string confirmationToken)
    {
        //TODO: implement email confirmation
        return true;
    }
}