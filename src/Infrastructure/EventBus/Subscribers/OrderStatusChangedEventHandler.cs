using Shop.Application.Events;

namespace Shop.Infrastructure.EventBus.Subscribers
{
    public sealed class OrderStatusChangedEventHandler : IEventHandler<OrderStatusChangedEvent>
    {
        private readonly ILogger<OrderStatusChangedEventHandler> _logger;

        public OrderStatusChangedEventHandler(ILogger<OrderStatusChangedEventHandler> logger) => _logger = logger;

        public Task HandleAsync(OrderStatusChangedEvent @event, CancellationToken ct = default)
        {
            _logger.LogInformation(
                "[OrderStatusChanged] Замовлення #{OrderId} для {Email}: {Old} ---> {New}.",
                @event.OrderId,
                @event.CustomerEmail,
                @event.OldStatus,
                @event.NewStatus);

            return Task.CompletedTask;
        }
    }
}
