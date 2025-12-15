using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Xunit;

namespace Shop.IntegrationTests
{
    public class ProductTests : BaseIntegrationTest
    {
        public ProductTests(SharedDatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddProduct_Should_SaveToDatabase()
        {
            var category = new Category { CategoryName = "Цукерки" };
            Context.Categories.Add(category);
            await Context.SaveChangesAsync();

            var product = new Product
            {
                ProductName = "Бджілка",
                Price = 120,
                ProductCountry = "Ukraine",
                StockQuantity = 100,
                Weight = 1
            };

            product.CategoryId = category.CategoryId;

            Context.Products.Add(product);
            await Context.SaveChangesAsync();

            var dbProduct = await Context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductName == "Бджілка");

            dbProduct.Should().NotBeNull();
            dbProduct!.Price.Should().Be(120);
            dbProduct.ProductId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task AddOrder_Should_Link_To_Customer()
        {
            var user = new Customer
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@gmail.com",
                PhoneNumber = "380639991199",
                DateOfBirth = new DateOnly(2000, 1, 1)
            };
            Context.Customers.Add(user);
            await Context.SaveChangesAsync();

            var order = new Order
            {
                CustomerId = user.CustomerId,
                OrderDate = DateOnly.FromDateTime(DateTime.Today)
            };
            Context.Orders.Add(order);
            await Context.SaveChangesAsync();

            var savedOrder = await Context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.CustomerId == user.CustomerId);

            savedOrder.Should().NotBeNull();
            savedOrder!.Customer.Email.Should().Be("testuser@gmail.com");
        }
    }
}