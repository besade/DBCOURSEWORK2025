using Shop.Models;

namespace Shop.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllActiveProductsAsync();
        Task<IEnumerable<Category>> GetAllActiveCategoriesAsync();
    }
}