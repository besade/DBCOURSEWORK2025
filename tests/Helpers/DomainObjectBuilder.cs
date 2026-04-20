using Shop.Domain.Models;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Tests.Helpers;

public static class DomainObjectBuilder
{
    public static Customer CreateCustomer(
        string firstName = "Test",
        string lastName = "User",
        string phone = "+380991234567",
        string email = "test@example.com",
        bool isAdmin = false)
    {
        return new Customer(
            firstName, lastName,
            new PhoneNumber(phone),
            new Email(email),
            new DateOnly(1995, 6, 15),
            new byte[64],
            new byte[64],
            isAdmin);
    }

    public static Address CreateAddress(
        int customerId = 1,
        bool isDefault = false,
        string country = "Ukraine",
        string city = "Kyiv",
        string line = "Khreschatyk 1")
        => new(customerId, country, city, line, isDefault);

    public static Product CreateProduct(
        string name = "Test Product",
        int categoryId = 1,
        int stock = 10,
        decimal price = 99.99m)
        => new(name, "Ukraine", 0.5m, price, stock, categoryId, new byte[16]);

    public static Category CreateCategory(string name = "Test Category")
        => new(name);

    public static Cart GetCartFor(Customer customer) => customer.Cart;
}