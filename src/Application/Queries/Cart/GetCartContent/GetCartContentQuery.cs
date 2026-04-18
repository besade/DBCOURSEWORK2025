using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Cart.GetCartContent
{
    public record GetCartContentQuery(int CustomerId) : IRequest<CartResponseDto>;
}
