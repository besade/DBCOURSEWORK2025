using Shop.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Infrastructure.Security
{
    internal class PasswordHasher : IPasswordHasher
    {
        public (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();

            var salt = hmac.Key;

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return (hash, salt);
        }

        public bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);

            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computed.SequenceEqual(hash);
        }
    }
}
