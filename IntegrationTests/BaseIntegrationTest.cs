using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Xunit;

namespace Shop.IntegrationTests
{
    [Collection("Database collection")]
    public abstract class BaseIntegrationTest : IDisposable
    {
        protected readonly ApplicationDbContext Context;
        private readonly SharedDatabaseFixture _fixture;

        protected BaseIntegrationTest(SharedDatabaseFixture fixture)
        {
            _fixture = fixture;
            Context = _fixture.CreateContext();

            Context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Context.ChangeTracker.Clear();
            Context.Database.RollbackTransaction();
            Context.Dispose();
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<SharedDatabaseFixture>
    {
    }
}