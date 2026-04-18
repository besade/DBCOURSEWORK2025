using MediatR;
using Shop.Domain.Models;

namespace Shop.Application.Commands.Order.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int OrderId, Status NewStatus) : IRequest;
}
