using Shop.Application.Interfaces;
using Shop.Application.IQueries;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderFactory _orderFactory;

        public OrderService(IOrderRepository orderRepository, IOrderReadRepository orderReadRepository, ICartRepository cartRepository, ICustomerRepository customerRepository,
            IProductRepository productRepository, IOrderFactory orderFactory)
        {
            _orderRepository = orderRepository;
            _orderReadRepository = orderReadRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderFactory = orderFactory;
        }
        
        // Create order
        public async Task CreateOrderAsync(int customerId, OrderRequestDto dto, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

            if (cart == null)
                throw new DomainNotFoundException(nameof(Cart), customerId);

            if (cart.CartItems == null) 
                throw new DomainNotFoundException(nameof(CartItem), cart);

            if (!cart.CartItems.Any())
                throw new DomainValidationException("Неможливо створити замовлення з пустим кошиком");

            var customer = await _customerRepository.GetByIdAsync(customerId, ct);

            var order = await _orderFactory.CreateAsync(cart, dto.AddressId, dto.Delivery, dto.CustomerIsRecipient, dto.RecipientFirstName, dto.RecipientLastName);

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, ct);

                if (product == null)
                    throw new DomainNotFoundException(nameof(Product), item.ProductId);

                product.UpdateStock(product.StockQuantity - item.Quantity);
                _productRepository.Update(product);
            }

            await _orderRepository.AddAsync(order, ct);

            customer!.Cart.Clear();

            await _orderRepository.SaveChangesAsync(ct);
        }

        // Get user orders
        public async Task<List<OrderResponseDto>> GetUserOrdersAsync(int customerId, CancellationToken ct)
        {
            return await _orderReadRepository.GetByCustomerIdAsync(customerId, ct);
        }

        // Get all orders
        public async Task<List<OrderResponseDto>> GetAllOrdersAsync(CancellationToken ct)
        {
            return await _orderReadRepository.GetAllAsync(ct);
        }

        // Update order status
        public async Task UpdateOrderStatusAsync(int orderId, Status newStatus, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, ct);

            if (order == null)
                throw new DomainNotFoundException(nameof(Order), orderId);

            order.ChangeStatus(newStatus);

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync(ct);
        }
    }
}