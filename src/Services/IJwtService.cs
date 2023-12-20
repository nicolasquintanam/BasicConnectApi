namespace BasicConnectApi.Services;

public interface IJwtService
{
    string GenerateToken(string name);
    void RevokeToken(string token);
}