using Shop.DTOs;
using Shop.Models;

namespace Shop.Services
{
    public interface IAdminService
    {
        // Products
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task CreateProductAsync(ProductDto dto);
        Task UpdateProductAsync(int id, ProductDto dto);
        Task DeleteProductAsync(int id);
        Task RestoreProductAsync(int id);

        // Categories
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryDto dto);
        Task UpdateCategoryAsync(int id, CategoryDto dto);
        Task DeleteCategoryAsync(int id);
        Task RestoreCategoryAsync(int id);
    }
}
