using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.DTOs;
using Shop.Models;
using Shop.Services;
using Xunit;

namespace Shop.UnitTests
{
    public class AuthServiceTests
    {
        private ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Login_Should_Return_User_When_Credentials_Are_Correct()
        {
            var context = CreateContext();
            var service = new AuthService(context);

            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));

            var user = new Customer
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@gmail.com",
                PhoneNumber = "+380000000000",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            context.Customers.Add(user);
            await context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var result = await service.LoginAsync(dto);

            result.Should().NotBeNull();
        }
    }
}