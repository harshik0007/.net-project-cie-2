using System;
using System.Security.Cryptography;

namespace CIE_2_PROJECT.Helpers
{
    public static class PasswordHasher
    {
        private const int Iterations = 150000;
        private const int SaltLength = 16;
        private const int HashLength = 32;
        private const string Version = "pbkdf2-sha1-v1";

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("A password is required.", "password");

            byte[] salt = new byte[SaltLength];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                random.GetBytes(salt);

            byte[] hash;
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, Iterations))
                hash = derive.GetBytes(HashLength);

            return Version + "$" + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string encoded)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(encoded))
                return false;

            string[] parts = encoded.Split('$');
            if (parts.Length != 4 || !string.Equals(parts[0], Version, StringComparison.Ordinal))
                return false;

            int iterations;
            byte[] salt;
            byte[] expected;
            try
            {
                iterations = int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException) { return false; }
            catch (OverflowException) { return false; }

            if (iterations < 100000 || iterations > 1000000 || salt.Length < 16 || expected.Length != HashLength)
                return false;

            byte[] actual;
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, iterations))
                actual = derive.GetBytes(expected.Length);

            int difference = 0;
            for (int i = 0; i < expected.Length; i++)
                difference |= expected[i] ^ actual[i];
            return difference == 0;
        }
    }
}
