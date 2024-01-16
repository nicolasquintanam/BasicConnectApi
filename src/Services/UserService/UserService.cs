namespace BasicConnectApi.Services;

using BasicConnectApi.Data;
using BasicConnectApi.Models;
using Microsoft.EntityFrameworkCore;

public class UserService(IApplicationDbContext dbContext, ITokenService tokenService, ILogger<UserService> logger) : IUserService
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<bool> ExistsUser(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        _logger.LogInformation("User exists? {0}", user is not null);
        return user is not null;
    }

    public async Task<UserResponse?> GetUserById(int userId)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return null;
        return new UserResponse()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<int?> GetUserId(string? email)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        return user?.Id;
    }

    public int? RegisterUser(string? firstName, string? lastName, string? email, string? password, bool isEmailConfirmed = false)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return null;
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

    public bool AuthenticateUser(string? email, string? password, out int id)
    {
        id = 0;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return false;
        var user = _dbContext.User.FirstOrDefault(u => u.Email == email && string.Equals(u.Password, password));
        if (user is null)
            return false;

        id = user.Id;
        return true;
    }

    public async Task<bool> ResetPassword(int userId, string? password)
    {
        if (string.IsNullOrEmpty(password))
            return false;
        var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return false;
        user.Password = password;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<UserResponse?> UpdateUser(int userId, string? firstName, string? lastName, string? email)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email))
            return null;

        email = email.ToLower();

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return null;
        user.FirstName = firstName;
        user.LastName = lastName;
        if (!email.Equals(user.Email))
        {
            user.Email = email;
            user.IsEmailConfirmed = false;
        }
        await _dbContext.SaveChangesAsync();
        return new UserResponse()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
