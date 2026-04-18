using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Cart.AddItem
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public AddItemToCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(AddItemToCartCommand command, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), command.CustomerId);

            cart.AddItem(command.ProductId);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
