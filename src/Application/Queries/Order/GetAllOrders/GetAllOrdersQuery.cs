using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Order.GetUserOrders
{
    public record GetAllOrdersQuery() : IRequest<OrdersListResponseDto>;
}
