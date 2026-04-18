using MediatR;

namespace Shop.Application.Commands.Cart.DecreaseCartItemQuantity
{
    public record DecreaseCartItemQuantityCommand(int CustomerId, int ProductId) : IRequest;
}
