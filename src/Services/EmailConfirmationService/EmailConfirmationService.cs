namespace BasicConnectApi.Services;

using BasicConnectApi.Data;

public class EmailConfirmationService : IEmailConfirmationService
{
    private readonly ApplicationDbContext _dbContext;

    public EmailConfirmationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendConfirmationEmailAsync(string userEmail)
    {
        //TODO: implement email confirmation sending
    }

    public bool ConfirmEmail(string userEmail, string confirmationToken)
    {
        var user = _dbContext.User.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
            return false;

        if (user.EmailConfirmationToken != confirmationToken)
            return false;

        user.IsEmailConfirmed = true;
        return true;
    }
}