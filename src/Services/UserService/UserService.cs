namespace BasicConnectApi.Services;

using BasicConnectApi.Data;
using BasicConnectApi.Models;
using Microsoft.EntityFrameworkCore;

public class UserService(IApplicationDbContext dbContext, ITokenService tokenService, ILogger<UserService> logger) : IUserService
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<bool> ExistsUser(string email)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        _logger.LogInformation("User exists? {0}", user is not null);
        return user is not null;
    }

    public async Task<int?> GetUserId(string email)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        return user?.Id;
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

    public async Task<bool> ResetPassword(int userId, string password)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return false;
        user.Password = password;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
