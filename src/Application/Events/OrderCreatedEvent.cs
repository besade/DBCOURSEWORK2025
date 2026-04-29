namespace Shop.Application.Events
{
    public record OrderCreatedEvent(int OrderId, int CustomerId, string CustomerEmail, string RecipientFullName,
    string DeliveryType, decimal TotalAmount, IReadOnlyList<OrderCreatedEvent.OrderLine> Items) : IntegrationEvent
    {
        public record OrderLine(int ProductId, string ProductName, int Quantity, decimal UnitPrice);
    }
}
