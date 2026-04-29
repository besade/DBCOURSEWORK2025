using Shop.Application.Events;

namespace Shop.Infrastructure.EventBus.Subscribers
{
    public sealed class LowStockDetectedEventHandler : IEventHandler<LowStockDetectedEvent>
    {
        private const int DefaultThreshold = 5;
        private readonly ILogger<LowStockDetectedEventHandler> _logger;

        public LowStockDetectedEventHandler(ILogger<LowStockDetectedEventHandler> logger) => _logger = logger;

        public Task HandleAsync(LowStockDetectedEvent @event, CancellationToken ct = default)
        {
            _logger.LogWarning(
                "[LowStock] Товар '{Name}' (ID={Id}): залишок {Stock} шт. <= порогу {Threshold}.",
                @event.ProductName,
                @event.ProductId,
                @event.CurrentStock,
                @event.Threshold);

            return Task.CompletedTask;
        }
    }
}
