using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

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
                "+380999999999",
                new DateOnly(2000, 1, 1),
                "AdminPassword123"
            );

            admin.MakeAdmin();

            await repository.AddAsync(admin);
            await repository.SaveChangesAsync();
        }
    }
}