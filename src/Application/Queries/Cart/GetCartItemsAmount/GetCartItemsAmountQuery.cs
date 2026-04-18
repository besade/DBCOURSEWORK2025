using MediatR;

namespace Shop.Application.Queries.Cart.GetCartItemsAmount
{
    public record GetCartItemsAmountQuery(int CustomerId) : IRequest<int>;
}
