using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.DTOs;
using Shop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Register
        public async Task RegisterAsync(RegisterDto dto)
        {
            if (await _db.Customers.AnyAsync(x => x.Email == dto.Email))
                throw new Exception("Email already exists");

            if (dto.Password.Length < 8)
                throw new Exception("Password must be at least 8 characters long");

            if (!dto.Password.Any(char.IsDigit))
                throw new Exception("Password must contain at least one digit");

            CreatePasswordHash(dto.Password, out var hash, out var salt);

            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        // Login
        public async Task<Customer?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Customers.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return null;

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        // Password Hash
        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }
    }
}