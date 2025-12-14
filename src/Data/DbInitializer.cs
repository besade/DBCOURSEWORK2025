using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Data
{
    public static class DbInitializer
    {
        public static void SeedAdmin(ApplicationDbContext context)
        {
            if (context.Customers.Any(c => c.IsAdmin))
                return;

            using var hmac = new HMACSHA512();
            var password = "12345678";

            var admin = new Customer
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                PhoneNumber = "0000000000",
                DateOfBirth = new DateOnly(2000, 1, 1),
                IsAdmin = true,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            };

            context.Customers.Add(admin);
            context.SaveChanges();
        }
    }
}