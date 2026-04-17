namespace Shop.Application.Interfaces
{
    public interface ICartService
    {
        Task AddItemAsync(int customerId, int productId, int quantity, CancellationToken ct);
        Task RemoveItemAsync(int customerId, int productId, CancellationToken ct);
        Task UpdateQuantityAsync(int customerId, int productId, int quantity, CancellationToken ct);
    }
}