namespace BasicConnectApi.Services;

public class EmailConfirmationService : IEmailConfirmationService
{
    public void SendConfirmationEmail(string userEmail)
    {
        //TODO: implement email confirmation sending
    }

    public bool ConfirmEmail(string userEmail, string confirmationToken)
    {
        //TODO: implement email confirmation
        return true;
    }
}