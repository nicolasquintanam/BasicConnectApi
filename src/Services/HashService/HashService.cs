using System.Security.Cryptography;
using System.Text;

namespace BasicConnectApi.Services;

public class HashService : IHashService
{
    public string HashData(string data)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
    }
}