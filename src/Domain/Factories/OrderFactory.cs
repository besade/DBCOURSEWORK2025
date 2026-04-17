using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;

namespace Shop.Domain.Factories
{
    public class OrderFactory : IOrderFactory
    {
        private readonly ICustomerRepository _customerRepository;

        public OrderFactory(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Order> CreateAsync(Cart cart, int addressId, DeliveryType delivery, bool customerIsRecipient, string? FirstName, string? LastName)
        {
            if (cart.CartItems == null || !cart.CartItems.Any())
                throw new DomainValidationException("Неможливо створити замовлення з порожнім кошиком.");

            if (customerIsRecipient == false && (FirstName == null || LastName == null))
                throw new DomainValidationException("Якщо Ви не отримувач - необхідно вказати ім'я та прізвище.");

            var customer = (await _customerRepository.GetByIdAsync(cart.CustomerId))!;

            string recipientFirstName = (customerIsRecipient ? customer.FirstName : FirstName)!;
            string recipientLastName = (customerIsRecipient ? customer.LastName : LastName)!;

            var order = new Order(cart.CustomerId, addressId, recipientFirstName, recipientLastName, delivery, customerIsRecipient);

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem(cartItem.ProductId, cartItem.Product.Price, cartItem.Quantity);

                order.OrderItems.Add(orderItem);
            }

            return order;
        }
    }
}
