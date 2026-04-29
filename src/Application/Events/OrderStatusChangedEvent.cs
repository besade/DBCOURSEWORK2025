namespace Shop.Application.Events
{
    public record OrderStatusChangedEvent(int OrderId, int CustomerId, string CustomerEmail, string OldStatus, string NewStatus) : IntegrationEvent;
}
