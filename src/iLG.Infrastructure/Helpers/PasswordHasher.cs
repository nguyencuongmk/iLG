using System.Security.Cryptography;
using System.Text;

namespace iLG.Infrastructure.Helpers
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            byte[] hash = HashPassword(password, salt, Iterations, HashSize);

            // Combine salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] hash = HashPassword(password, salt, Iterations, HashSize);

            // Compare the computed hash with the stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt, int iterations, int outputSize)
        {
            using (var pbkdf2 = new HMACSHA256(salt))
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password);

                // Hash the password
                byte[] hash = pbkdf2.ComputeHash(inputBytes);

                // Perform additional iterations
                for (int i = 1; i < iterations; i++)
                {
                    hash = pbkdf2.ComputeHash(hash);
                }

                return hash;
            }
        }

        public static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder password = new();
            Random random = new();

            while (password.Length < length)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }
    }
}