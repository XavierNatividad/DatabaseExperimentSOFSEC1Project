using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Security.AccessControl;
using System.IO;

namespace GradeCalculator
{
    class GPAwareCryptography
    {
        private const int saltSize = 16;
        private const int hashSize = 32;
        private const int degreesOfParallelism = 4;
        private const int iterations = 4;
        private const int memorySize = 1024 * 1024;

        public string HashPassword(string password)
        {
            byte[] salt = new byte[saltSize];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(salt);
            }

            byte[] hash = HashPassword(password, salt);

            var saltHash = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, saltHash, 0, salt.Length);
            Array.Copy(hash, 0, saltHash, salt.Length, hash.Length);

            return Convert.ToBase64String(saltHash);
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = degreesOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            return argon2.GetBytes(hashSize);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[saltSize];
            byte[] hash = new byte[hashSize];
            Array.Copy(combinedBytes, 0, salt, 0, saltSize);
            Array.Copy(combinedBytes, saltSize, hash, 0, hashSize);

            byte[] newHash = HashPassword(password, salt);

            return hash.Equals(newHash);
        }

        public static byte[] Encrypt(string password, string plainText)
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

                            return GetEncryptedDataIncludedIV(memoryStream.ToArray(), aes.IV);
                        }
                    }

                }

            }

        }
        public static string Decrypt(string password, byte[] cipherText)
        {
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

                    using (MemoryStream memoryStream = new MemoryStream())
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
            byte[] cipherText = new byte[encryptedDataIncludingIV.Length -16];
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
