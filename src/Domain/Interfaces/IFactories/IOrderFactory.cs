using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IFactories
{
    public interface IOrderFactory
    {
        Task<Order> CreateAsync(Cart cart, int addressId, DeliveryType delivery, bool customerIsRecipient, string FirstName, string LastName);
    }
}
