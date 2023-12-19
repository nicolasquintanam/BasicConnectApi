namespace BasicConnectApi.Services;

using BasicConnectApi.Data;
using BasicConnectApi.Models;

public class UserService : IUserService
{
    private ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int RegisterUser(string name, string email, string password)
    {
        var user = new User()
        {
            Name = name,
            Email = email,
            Password = password
        };

        _dbContext.User.Add(user);
        _dbContext.SaveChanges();

        return user.Id;
    }
}
