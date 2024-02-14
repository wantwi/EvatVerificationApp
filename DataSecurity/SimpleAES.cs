using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EvatVerificationApp.DataSecurity
{
    internal static class SimpleAES
    {
        public static string Password = "ug4Zc3n9CwB9CrrcrLhkax5dSWPQvNET"; 
        public static string IV = "jVSxL2lfT5TgGh6r";
        public static string Salt = "42878BUFB061";

        public static string Key128 = "KMDINLOPSNKEKNEF";
     public static string initial128 = "LDHSMXCVIJIDFAON";

        public static string EncryptPasswordBased(string plainText, byte[] key, byte[] iv)
        {
            using (Aes cipher = Aes.Create())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.Key = key;
                cipher.IV = iv;

                ICryptoTransform encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV);

                byte[] encryptedBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string DecryptPasswordBased128(string cipherText)
        {
          



            var Key = System.Text.Encoding.UTF8.GetBytes(Key128);
            var IV = System.Text.Encoding.UTF8.GetBytes(initial128);
            //var cipherText = System.Text.Encoding.UTF8.GetBytes(cipherText2);



            var tx = Convert.FromBase64String(cipherText);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key,
                                             rijAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(tx))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                           decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }



            }



            return plaintext;
        }

        public static string DecryptPasswordBased(string cipherText, byte[] key, byte[] iv)
        {
            using (Aes cipher = Aes.Create())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.Key = key;
                cipher.IV = iv;

                ICryptoTransform decryptor = cipher.CreateDecryptor(cipher.Key, cipher.IV);

                byte[] encryptedBytes = Convert.FromBase64String(cipherText);

                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static byte[] GenerateIv(string iv)
        {
            return Encoding.UTF8.GetBytes(iv);
        }

        public static byte[] GetKeyFromPassword(string password, string salt)
        {
            using (var factory = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 65536, HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = factory.GetBytes(256 / 8);
                return keyBytes;
            }
        }

    }
}