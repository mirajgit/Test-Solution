namespace Test.UI
{
    public class EncryptionHelper
    {
        private static readonly string key = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%^&*()_-+=<>?";

        private static readonly int AESKeySize = 32; // AES-256 (32 bytes)
        private static readonly int AESBlockSize = 128; // AES block size in bits (16 bytes)

        public static string Encrypt(string plainText)
        {
            using (var aesAlg = System.Security.Cryptography.Aes.Create())
            {
                // Ensure the key is exactly 32 bytes (AES-256)
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(key.PadRight(AESKeySize, ' ').Substring(0, AESKeySize));
                aesAlg.IV = GenerateRandomIV(); // Generate a random IV

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                        {
                            using (var sw = new System.IO.StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }

                        byte[] encryptedData = ms.ToArray();
                        byte[] result = new byte[aesAlg.IV.Length + encryptedData.Length];

                        // Prepend IV to the encrypted data
                        Array.Copy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
                        Array.Copy(encryptedData, 0, result, aesAlg.IV.Length, encryptedData.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Extract the IV from the beginning of the cipher text
            byte[] iv = new byte[16]; // 16 bytes IV for AES
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);

            byte[] encryptedData = new byte[cipherBytes.Length - iv.Length];
            Array.Copy(cipherBytes, iv.Length, encryptedData, 0, encryptedData.Length);

            using (var aesAlg = System.Security.Cryptography.Aes.Create())
            {
                // Ensure the key is exactly 32 bytes (AES-256)
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(key.PadRight(AESKeySize, ' ').Substring(0, AESKeySize));
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var ms = new System.IO.MemoryStream(encryptedData))
                    {
                        using (var cs = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                        {
                            using (var sr = new System.IO.StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GenerateRandomIV()
        {
            var iv = new byte[16]; 
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }
    }
}
