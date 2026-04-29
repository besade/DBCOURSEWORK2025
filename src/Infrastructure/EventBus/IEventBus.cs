using Shop.Application.Interfaces;

namespace Shop.Infrastructure.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : IIntegrationEvent;
    }
}
