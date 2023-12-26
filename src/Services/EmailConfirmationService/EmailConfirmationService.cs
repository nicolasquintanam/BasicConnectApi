namespace BasicConnectApi.Services;

using BasicConnectApi.Data;

public class EmailConfirmationService : IEmailConfirmationService
{
    const string APP_NAME = "BasicConnect";
    const string URL_APP = "http://localhost:5001";
    const string ENDPOINT_CONFIRM = "/api/v1/emailconfirmation/confirm";


    private readonly IApplicationDbContext _dbContext;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ITokenService _tokenService;

    public EmailConfirmationService(IApplicationDbContext dbContext, IEmailSenderService emailSenderService, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _emailSenderService = emailSenderService;
        _tokenService = tokenService;
    }

    public async Task SendConfirmationEmailAsync(string userEmail)
    {
        var user = _dbContext.User.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
            return;
        var confirmationToken = user.EmailConfirmationToken;
        if (string.IsNullOrEmpty(confirmationToken))
            confirmationToken = _tokenService.GenerateToken(50);

        await SendConfirmationEmail(userEmail, user.FirstName, confirmationToken);
    }

    public bool ConfirmEmail(string userEmail, string confirmationToken)
    {
        var user = _dbContext.User.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
            return false;

        if (user.EmailConfirmationToken != confirmationToken)
            return false;

        user.IsEmailConfirmed = true;
        _dbContext.SaveChanges();
        return true;
    }


    private async Task SendConfirmationEmail(string email, string name, string confirmationToken)
    {
        string subject = "Confirm Your Email Address";
        string confirmationLink = $"{URL_APP}{ENDPOINT_CONFIRM}?email={email}&token={confirmationToken}";
        string body = $"Hello {name},\n\n" +
                      $"Thank you for registering with {APP_NAME}. To complete the registration process, we need you to confirm your email address.\n\n" +
                      $"Please click the following link to confirm your email:\n{confirmationLink}\n\n" +
                      $"If you did not create an account on {APP_NAME}, you can ignore this email.\n\n" +
                      $"Thanks,\nThe {APP_NAME} Team";
        await _emailSenderService.SendEmail(email, subject, body);
    }
}