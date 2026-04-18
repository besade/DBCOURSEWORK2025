using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Cart.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateCartItemQuantityCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken ct)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, ct);

            if (cart == null)
                throw new NotFoundException(nameof(Cart), request.CustomerId);

            cart.UpdateItemQuantity(request.ProductId, request.NewQuantity);

            await _cartRepository.SaveChangesAsync(ct);
        }
    }
}
