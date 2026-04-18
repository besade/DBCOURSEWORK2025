using MediatR;

namespace Shop.Application.Commands.Cart.UpdateCartItemQuantity
{
    public record UpdateCartItemQuantityCommand(int CustomerId, int ProductId, int NewQuantity) : IRequest;
}
