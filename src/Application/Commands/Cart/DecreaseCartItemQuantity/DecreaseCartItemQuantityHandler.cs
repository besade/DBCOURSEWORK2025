using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Cart.DecreaseCartItemQuantity
{
    public class DecreaseCartItemQuantityCommandHandler : IRequestHandler<DecreaseCartItemQuantityCommand>
    {
        private readonly ICartRepository _cartRepository;

        public DecreaseCartItemQuantityCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(DecreaseCartItemQuantityCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
            {
                throw new NotFoundException(nameof(Cart), command.CustomerId);
            }

            cart.DecreaseItemQuantity(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
