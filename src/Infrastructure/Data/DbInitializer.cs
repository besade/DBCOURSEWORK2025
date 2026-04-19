using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(ICustomerRepository repository, ICustomerFactory factory)
        {
            if (await repository.AdminExistsAsync())
                return;

            var admin = await factory.CreateAsync(
                "Admin",
                "System",
                "admin@gmail.com",
                "0000000000",
                new DateOnly(2000, 1, 1),
                "AdminPassword123"
            );

            admin.MakeAdmin();

            await repository.AddAsync(admin);
            await repository.SaveChangesAsync();
        }
    }
}