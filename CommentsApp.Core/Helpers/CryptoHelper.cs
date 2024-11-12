using System.Security.Cryptography;
using System.Text;

namespace CommentsApp.Core.Helpers;

public static class CryptoHelper
{
    public static byte[] GetPbkdf2Bytes(string data, byte[]? salt = null, int iterations = 10_000,
        HashAlgorithmName? algorithmName = null, int sizeInBytes = 32)
    {
        return GetPbkdf2Bytes(Encoding.UTF8.GetBytes(data.ToLowerInvariant()), salt, iterations, algorithmName, sizeInBytes);
    }
    public static byte[] GetPbkdf2Bytes(byte[] data, byte[]? salt = null, int iterations = 10_000,
        HashAlgorithmName? algorithmName = null, int sizeInBytes = 32)
    {
        return Rfc2898DeriveBytes.Pbkdf2(data, salt ?? [], iterations, algorithmName ?? HashAlgorithmName.SHA256, sizeInBytes);
    }

    public static string GenerateRandomString(int sizeInBytes = 16)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(sizeInBytes));
    }
}