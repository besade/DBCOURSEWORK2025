using MediatR;
using Shop.Application.Events;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Infrastructure.EventBus;

namespace Shop.Application.Commands.Order.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventBus _eventBus;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateOrderStatusCommand command, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(command.OrderId, ct);

            if (order == null)
                throw new NotFoundException(nameof(Order), command.OrderId);

            var oldStatus = order.OrderStatus.ToString();
            order.ChangeStatus(command.NewStatus);

            await _orderRepository.SaveChangesAsync(ct);

            var customer = await _customerRepository.GetByIdAsync(order.CustomerId, ct);
            var email = customer?.Email.Value ?? string.Empty;

            await _eventBus.PublishAsync(new OrderStatusChangedEvent(
                order.OrderId,
                order.CustomerId,
                email,
                oldStatus,
                command.NewStatus.ToString()), ct);
        }
    }
}
