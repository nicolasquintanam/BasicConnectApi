namespace BasicConnectApi.Services;

using System.Security.Cryptography;

public class TokenService : ITokenService
{
    public string GenerateToken(int length)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] chars = new char[length];

        using (var crypto = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[length];
            crypto.GetBytes(data);

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[data[i] % allowedChars.Length];
            }
        }
        return new string(chars);
    }
}