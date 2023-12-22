namespace BasicConnectApi.Services;

public interface IUserService
{
    int RegisterUser(string firstName, string lastName, string email, string password, bool isEmailConfirmed = false);
    bool AuthenticateUser(string email, string password, out int id);
    bool ExistsUser(string email);
}
