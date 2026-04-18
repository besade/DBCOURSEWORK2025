using MediatR;

namespace Shop.Application.Commands.Cart.RemoveItemFromCart
{
    public record RemoveItemFromCartCommand(int CustomerId, int ProductId) : IRequest;
}
