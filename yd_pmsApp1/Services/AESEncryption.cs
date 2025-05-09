using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESEncryption
{
    public static string Encrypt(string plainText, string keyStr = "abcdefgabcdefg12")
    {
        using (Aes aes = Aes.Create())
        {
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(keyStr);

            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] inputBuffer = Encoding.UTF8.GetBytes(plainText);
            byte[] outputBuffer = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Convert.ToBase64String(outputBuffer);
        }
    }
}

