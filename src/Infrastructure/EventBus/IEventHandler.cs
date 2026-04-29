using Shop.Application.Interfaces;

namespace Shop.Infrastructure.EventBus
{
    public interface IEventHandler<in T> where T : IIntegrationEvent
    {
        Task HandleAsync(T @event, CancellationToken ct = default);
    }
}
