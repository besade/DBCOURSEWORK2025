using Shop.Application.Events;

namespace Shop.Infrastructure.EventBus.Subscribers
{
    public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) => _logger = logger;

        public Task HandleAsync(OrderCreatedEvent @event, CancellationToken ct = default)
        {
            _logger.LogInformation(
                "[OrderCreated] Замовлення #{OrderId} для {Email}. " +
                "Отримувач: {Recipient}. Доставка: {Delivery}. Сума: {Total:N2} ₴. " +
                "Товарів: {ItemCount}.",
                @event.OrderId,
                @event.CustomerEmail,
                @event.RecipientFullName,
                @event.DeliveryType,
                @event.TotalAmount,
                @event.Items.Count);

            return Task.CompletedTask;
        }
    }
}
