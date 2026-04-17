using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IFactories
{
    public interface ICustomerFactory
    {
        Task<Customer> CreateAsync(string firstName, string lastName, string email, string phone, DateOnly birthdayDate, string password);
    }
}
