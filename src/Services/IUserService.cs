namespace BasicConnectApi.Services;

public interface IUserService
{
    int RegisterUser(string name, string email, string password);
}
