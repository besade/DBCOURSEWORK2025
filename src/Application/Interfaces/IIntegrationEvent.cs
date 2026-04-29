namespace Shop.Application.Interfaces
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }
}
