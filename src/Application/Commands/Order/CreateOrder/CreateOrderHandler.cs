using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderFactory _orderFactory;

        public CreateOrderCommandHandler(ICartRepository cartRepository, IOrderRepository orderRepository, IProductRepository produtctRepository, IOrderFactory orderFactory)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = produtctRepository;
            _orderFactory = orderFactory;
        }

        public async Task<int> Handle(CreateOrderCommand command, CancellationToken ct)
        {
            var dto = command.Dto;

            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), command.CustomerId);

            var order = await _orderFactory.CreateAsync(cart, dto.AddressId, dto.Delivery, dto.CustomerIsRecipient, dto.RecipientFirstName, dto.RecipientLastName);

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, ct);

                if (product == null)
                    throw new NotFoundException(nameof(Product), item.ProductId);

                product.UpdateStock(product.StockQuantity - item.Quantity);
            }

            await _orderRepository.AddAsync(order, ct);

            cart.Clear();

            await _orderRepository.SaveChangesAsync(ct);

            return order.OrderId;
        }
    }
}
