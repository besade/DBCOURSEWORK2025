using Shop.Models;

namespace Shop.Services
{
    public interface ICartService
    {
        Task AddItemAsync(int productId, int quantity);
        Task RemoveItemAsync(int productId);
        Task UpdateQuantityAsync(int productId, int quantity);
        Task<int> GetCartItemCountAsync();
        Task<Dictionary<int, int>> GetCartContentsAsync();
        Task<Cart?> GetCartAsync();
    }
}