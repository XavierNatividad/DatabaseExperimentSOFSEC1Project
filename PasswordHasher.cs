using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Security.AccessControl;

namespace GradeCalculator
{
    class PasswordHasher
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

            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
