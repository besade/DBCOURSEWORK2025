using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Cart.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveItemFromCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(RemoveItemFromCartCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), command.CustomerId);

            cart.RemoveItem(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
