using MediatR;

namespace Shop.Application.Commands.Cart.AddItem
{
    public record AddItemToCartCommand(int CustomerId, int ProductId) : IRequest;
}
