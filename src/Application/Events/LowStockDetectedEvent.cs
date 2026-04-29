namespace Shop.Application.Events
{
    public record LowStockDetectedEvent(int ProductId, string ProductName, int CurrentStock, int Threshold) : IntegrationEvent;
}
