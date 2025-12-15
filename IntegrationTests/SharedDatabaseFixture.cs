using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Testcontainers.PostgreSql;

namespace Shop.IntegrationTests
{
    public class SharedDatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("shop_test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public string ConnectionString => _dbContainer.GetConnectionString();
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var context = CreateContext();
            await context.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}