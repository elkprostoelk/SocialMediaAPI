using SocialMediaAPI.Core.Interfaces;
using System.Security.Cryptography;

namespace SocialMediaAPI.Core.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10_000;

        public (string salt, string hash) HashPassword(string password, string? oldSalt = null)
        {
            byte[] salt;
            if (string.IsNullOrWhiteSpace(oldSalt))
            {
                salt = new byte[SaltSize];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
            }
            else
            {
                salt = Convert.FromBase64String(oldSalt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }
    }
}
