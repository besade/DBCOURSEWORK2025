using MediatR;

namespace Shop.Application.Commands.Cart.IncreaseCartItemQuantity
{
    public record IncreaseCartItemQuantityCommand(int CustomerId, int ProductId) : IRequest;
}
