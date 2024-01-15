namespace BasicConnectApi.Services;

using BasicConnectApi.Enums;

public interface IJwtService
{
    string GenerateToken(string name, TokenTypeEnum tokenType = TokenTypeEnum.AccessToken);
    void RevokeToken(string token);
    bool TokenIsRevoked(string token);
    string? GetTokenFromAuthorizationHeader(IHeaderDictionary headers);
    string? GetJtiFromToken(string token);
    int? GetUserIdFromToken(string token);
    bool ValidateTemporaryToken(string token);
    bool ValidateAccessToken(string token);
}