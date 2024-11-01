using BankingAppBackend.Model;
using Microsoft.AspNetCore.Identity;

namespace BankingAppBackend.Utilities
{

    public static class Hasher
    {
        // Static instance of PasswordHasher
        private static readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

        // Hashes the password and returns the hashed value
        public static string HashPassword(string email, string password)
        {
            return _passwordHasher.HashPassword(("el135" + email), password);
        }

        // Verifies the hashed password against the provided plain password
        public static PasswordVerificationResult VerifyPassword(string email, string password, string hashedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(("el135" + email), hashedPassword, password);
        }
    }
}
