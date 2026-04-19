using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Infrastructure.Repositories;

namespace Shop.Application.Commands.Cart.AddItem
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public AddItemToCartCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task Handle(AddItemToCartCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), command.CustomerId);

            var product = await _productRepository.GetByIdAsync(command.ProductId, ct);
            if (product == null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            if (product.StockQuantity == 0)
            {
                throw new DomainValidationException($"Недостатньо товару на складі.");
            }

            cart.AddItem(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
