using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;

namespace Shop.Application.Commands.Cart.IncreaseCartItemQuantity
{
    public class IncreaseCartItemQuantityCommandHandler : IRequestHandler<IncreaseCartItemQuantityCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public IncreaseCartItemQuantityCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task Handle(IncreaseCartItemQuantityCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
            {
                throw new NotFoundException(nameof(Cart), command.CustomerId);
            }

            var product = await _productRepository.GetByIdAsync(command.ProductId, ct);
            if (product == null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == command.ProductId);
            if (cartItem == null)
                throw new NotFoundException(nameof(CartItem), command.ProductId);

            if (cartItem.Quantity + 1 > product.StockQuantity)
            {
                throw new DomainValidationException($"Недостатньо товару на складі. Доступно: {product.StockQuantity}");
            }

            cart.IncreaseItemQuantity(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
