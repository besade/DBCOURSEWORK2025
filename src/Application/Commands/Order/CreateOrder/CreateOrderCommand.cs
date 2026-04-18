using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Order.CreateOrder
{
    public record CreateOrderCommand(int CustomerId, OrderRequestDto Dto) : IRequest<int>;
}
