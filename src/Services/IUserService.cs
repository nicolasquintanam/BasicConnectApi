namespace BasicConnectApi.Services;

public interface IUserService
{
    int RegisterUser(string firstName, string lastName, string email, string password);
    bool AuthenticateUser(string email, string password, out int id);
}
