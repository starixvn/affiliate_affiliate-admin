using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.AffiliateAdmin.Core
{
    public class Crypt
    {
        public Crypt()
        {
            // Default constructor
        }

        // Call following function to encrypt data
        public string Encrypt(string clearText, string password)
        {
            // Convert text to bytes
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);

            // We will derieve our Key and Vectore based on following
            // password and a random salt value, 13 bytes in size.
            byte[] deriveBytes = new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            };
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, deriveBytes);

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            byte[] encryptedData;

            // Create stream for encryption
            using (MemoryStream ms = new MemoryStream())
            {
                // Create Rijndael object with key and vector
                using (Rijndael alg = Rijndael.Create())
                {
                    alg.Key = key;
                    alg.IV = iv;

                    // Forming cryptostream to link with data stream.
                    using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write all data to stream.
                        cs.Write(clearData, 0, clearData.Length);
                    }

                    encryptedData = ms.ToArray();
                }
            }

            return encryptedData;
        }

        // Call following function to decrypt data
        public string Decrypt(string cipherText, string password)
        {
            // Convert base 64 text to bytes
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // We will derieve our Key and Vectore based on following
            // password and a random salt value, 13 bytes in size.
            byte[] deriveBytes = new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            };
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, deriveBytes);
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Converting unicode string from decrypted data
            return Encoding.Unicode.GetString(decryptedData);
        }

        public byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            byte[] decryptedData;

            // Create stream for decryption
            using (MemoryStream ms = new MemoryStream())
            {
                // Create Rijndael object with key and vector
                using (Rijndael alg = Rijndael.Create())
                {
                    alg.Key = key;
                    alg.IV = iv;

                    // Forming cryptostream to link with data stream.
                    using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // Write all data to stream.
                        cs.Write(cipherData, 0, cipherData.Length);
                    }

                    decryptedData = ms.ToArray();
                }
            }

            return decryptedData;
        }
    }
}