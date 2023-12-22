namespace BasicConnectApi.Services;

public interface ITokenService
{
    string GenerateToken(int length);
}