namespace BasicConnectApi.Services;

using BasicConnectApi.Data;
using BasicConnectApi.Models;

public class UserService : IUserService
{
    private ApplicationDbContext _dbContext;
    private ITokenService _tokenService;

    public UserService(ApplicationDbContext dbContext, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public bool ExistsUser(string email)
    {
        var user = _dbContext.User.FirstOrDefault(u => u.Email == email);
        return user is not null;
    }

    public int RegisterUser(string firstName, string lastName, string email, string password, bool isEmailConfirmed)
    {
        var user = new User()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            IsEmailConfirmed = isEmailConfirmed
        };

        if (!isEmailConfirmed)
            user.EmailConfirmationToken = _tokenService.GenerateToken(50);

        _dbContext.User.Add(user);
        _dbContext.SaveChanges();

        return user.Id;
    }

    public bool AuthenticateUser(string email, string password, out int id)
    {
        id = 0;
        var user = _dbContext.User.FirstOrDefault(u => u.Email == email && string.Equals(u.Password, password));
        if (user is null)
            return false;

        id = user.Id;
        return true;
    }
}
