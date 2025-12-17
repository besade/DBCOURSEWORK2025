using Shop.Models;

namespace Shop.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, int pageSize);
        Task<IEnumerable<Category>> GetAllActiveCategoriesAsync();
    }
}