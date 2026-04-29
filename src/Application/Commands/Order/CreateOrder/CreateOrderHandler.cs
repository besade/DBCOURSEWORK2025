using MediatR;
using Shop.Application.Events;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Infrastructure.EventBus;

namespace Shop.Application.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private const int LowStockThreshold = 5;

        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderFactory _orderFactory;
        private readonly IEventBus _eventBus;

        public CreateOrderCommandHandler(ICartRepository cartRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository, 
            IProductRepository produtctRepository, IOrderFactory orderFactory, IEventBus eventBus)
        {
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = produtctRepository;
            _orderFactory = orderFactory;
            _eventBus = eventBus;
        }

        public async Task<int> Handle(CreateOrderCommand command, CancellationToken ct)
        {
            var dto = command.Dto;

            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), command.CustomerId);

            var order = await _orderFactory.CreateAsync(cart, dto.AddressId, dto.Delivery, dto.CustomerIsRecipient, dto.RecipientFirstName, dto.RecipientLastName);

            var lowStockEvents = new List<LowStockDetectedEvent>();

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, ct);

                if (product == null)
                    throw new NotFoundException(nameof(Product), item.ProductId);

                product.UpdateStock(product.StockQuantity - item.Quantity);

                if (product.StockQuantity <= LowStockThreshold)
                {
                    lowStockEvents.Add(new LowStockDetectedEvent(
                        product.ProductId,
                        product.ProductName,
                        product.StockQuantity,
                        LowStockThreshold));
                }
            }

            await _orderRepository.AddAsync(order, ct);

            cart.Clear();

            await _orderRepository.SaveChangesAsync(ct);

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId, ct);
            var email = customer?.Email.Value ?? string.Empty;

            var lines = order.OrderItems.Select(oi => new OrderCreatedEvent.OrderLine(
                oi.ProductId,
                oi.Product?.ProductName ?? string.Empty,
                oi.Quantity,
                oi.UnitPrice)).ToList();

            await _eventBus.PublishAsync(new OrderCreatedEvent(
                order.OrderId,
                command.CustomerId,
                email,
                $"{order.RecipientFirstName} {order.RecipientLastName}",
                order.Delivery.ToString(),
                order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                lines), ct);

            foreach (var evt in lowStockEvents)
                await _eventBus.PublishAsync(evt, ct);

            return order.OrderId;
        }
    }
}
