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
