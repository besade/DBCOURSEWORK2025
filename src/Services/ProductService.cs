using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _db.Products.FindAsync(id);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, int pageSize)
    {
        var query = _db.Products
            .Include(p => p.Category)
            .Where(p => p.isDeleted == false);

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        int totalCount = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.ProductId) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<IEnumerable<Category>> GetAllActiveCategoriesAsync()
    {
        return await _db.Categories
            .Where(p => p.isDeleted == false)
            .ToListAsync();
    }
}