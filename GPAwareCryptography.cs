using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Security.AccessControl;
using System.IO;
using SOFSEC1_Project;
using DatabaseExperimentSOFSEC1Project;

namespace GradeCalculator
{
    class GPAwareCryptography
    {
        private const int saltSize = 16;
        private const int hashSize = 32;
        private const int degreesOfParallelism = 4;
        private const int iterations = 4;
        private const int memorySize = 1024 * 1024;
        public byte[] CreateSalt()
        {
            var salt = new byte[saltSize];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            return salt;
        }
        public byte[] HashPassword(string password, byte[] salt)
        {
            var argon2id = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = degreesOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            byte[] preSaltPassword = argon2id.GetBytes(hashSize);
            byte[] saltPassword = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, saltPassword, 0, saltSize);
            Array.Copy(preSaltPassword, 0, saltPassword, saltSize, hashSize);
            return saltPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] combinedBytes = GetBytesFromString(hashedPassword);

            byte[] salt = new byte[saltSize];
            byte[] hash = new byte[hashSize];
            Array.Copy(combinedBytes, 0, salt, 0, saltSize);
            Array.Copy(combinedBytes, saltSize, hash, 0, hashSize);

            byte[] newHash = HashPassword(password, salt);

            // Extract the hash part from the newHash
            byte[] newHashPart = new byte[hashSize];
            Array.Copy(newHash, saltSize, newHashPart, 0, hashSize);

            return hash.SequenceEqual(newHashPart);
        }

        public static string Encrypt(string password, string plainText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;

                    aes.GenerateIV();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cryptStream))
                            {
                                writer.Write(plainText);
                            }

                            return BytesArrayToString(GetEncryptedDataIncludedIV(memoryStream.ToArray(), aes.IV));
                        }
                    }

                }

            }

        }
        public static string Decrypt(string password, string stringCipherText)
        {
            byte[] cipherText = GetBytesFromString(stringCipherText);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;

                    byte[] tempCypherText;

                    (aes.IV, tempCypherText) = GetIV(cipherText);
                    cipherText = tempCypherText;

                    using (MemoryStream memoryStream = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cryptStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cryptStream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private static byte[] GetEncryptedDataIncludedIV(byte[] encryptedData, byte[] iv)
        {
            byte[] result = new byte[iv.Length + encryptedData.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);
            return result;
        }
        public static (byte[], byte[]) GetIV(byte[] encryptedDataIncludingIV)
        {
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[encryptedDataIncludingIV.Length - 16];
            Buffer.BlockCopy(encryptedDataIncludingIV, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedDataIncludingIV, iv.Length, cipherText, 0, cipherText.Length);
            return (iv, cipherText);
        }

        public static string BytesArrayToString(byte[] cipherText)
        {
            return Convert.ToBase64String(cipherText);
        }

        public static byte[] GetBytesFromString(string cipherText)
        {
            return Convert.FromBase64String(cipherText);
        }
    }
}
