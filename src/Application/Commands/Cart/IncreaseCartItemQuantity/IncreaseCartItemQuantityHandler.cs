using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Cart.IncreaseCartItemQuantity
{
    public class IncreaseCartItemQuantityCommandHandler : IRequestHandler<IncreaseCartItemQuantityCommand>
    {
        private readonly ICartRepository _cartRepository;

        public IncreaseCartItemQuantityCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(IncreaseCartItemQuantityCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
            {
                throw new NotFoundException(nameof(Cart), command.CustomerId);
            }

            cart.IncreaseItemQuantity(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
