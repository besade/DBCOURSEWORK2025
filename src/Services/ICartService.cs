using Shop.Models;

namespace Shop.Services
{
    public interface ICartService
    {
        Task AddItemAsync(int customerId, int productId, int quantity);
        Task RemoveItemAsync(int customerId, int productId);
        Task UpdateQuantityAsync(int customerId, int productId, int quantity);
        Task<int> GetCartItemCountAsync(int customerId);
        Task<Dictionary<int, int>> GetCartContentsAsync(int customerId);
        Task<Cart?> GetCartAsync(int customerId);
    }
}