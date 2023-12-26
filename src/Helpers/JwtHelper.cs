using System.IdentityModel.Tokens.Jwt;

namespace BasicConnectApi.Helpers;

public class JwtHelper
{
    public static string? GetTokenFromAuthorizationHeader(IHeaderDictionary headers)
    {
        if (headers.TryGetValue("Authorization", out var authorizationHeader) &&
            !string.IsNullOrWhiteSpace(authorizationHeader) &&
            authorizationHeader.ToString().StartsWith("Bearer "))
        {
            return authorizationHeader.ToString().Substring("Bearer ".Length);
        }

        return null;
    }

    public static string? GetJti(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var jti = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "jti").ToString().Substring("jti: ".Length);

        return jti;
    }

    public static int? GetUserId(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid");

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }
}