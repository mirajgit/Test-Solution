using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Test.UI
{
    public static class UrlCryptoHelper
    {
        private static readonly string Key = "A9F3B1C7D5E9G2H6K4M7N0P2Q5S8U1R3"; // 32 chars = 256-bit key
        private static readonly string IV = "Z8Y6X5W4V3U2T1S0";                 // 16 chars = 128-bit IV
        public static string Encrypt(string plainText)
        {
            // URL encode the plainText before encrypting it
            string urlEncodedPlainText = Uri.EscapeDataString(plainText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(urlEncodedPlainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return WebEncoders.Base64UrlEncode(encryptedBytes); // URL-safe Base64
        }

        public static string Decrypt(string encryptedText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var encryptedBytes = WebEncoders.Base64UrlDecode(encryptedText);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            // URL decode the decrypted text
            return Uri.UnescapeDataString(decryptedText);
        }

        //public static string Decrypt(string encryptedText)
        //{
        //    using var aes = Aes.Create();
        //    aes.Key = Encoding.UTF8.GetBytes(Key);
        //    aes.IV = Encoding.UTF8.GetBytes(IV);
        //    aes.Padding = PaddingMode.PKCS7;

        //    using var decryptor = aes.CreateDecryptor();
        //    var encryptedBytes = WebEncoders.Base64UrlDecode(encryptedText);
        //    var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        //    return Encoding.UTF8.GetString(decryptedBytes);
        //}
    }
}
