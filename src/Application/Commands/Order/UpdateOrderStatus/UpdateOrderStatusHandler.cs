using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Order.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(UpdateOrderStatusCommand command, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(command.OrderId, ct);

            if (order == null)
                throw new NotFoundException(nameof(Order), command.OrderId);

            order.ChangeStatus(command.NewStatus);

            await _orderRepository.SaveChangesAsync(ct);
        }
    }
}
