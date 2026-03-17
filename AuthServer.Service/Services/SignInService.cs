using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Service.Services;

public static class SignInService 
{
    public static SecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }

    public static string DecryptToken(string encryptedToken, string encryptionKey)
    {
        var parts = encryptedToken.Split(':');
        if (parts.Length != 2)
            throw new FormatException("Invalid encrypted token format.");

        var iv = Convert.FromBase64String(parts[0]);
        var cipherText = Convert.FromBase64String(parts[1]);
        var key = Encoding.UTF8.GetBytes(encryptionKey);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}

