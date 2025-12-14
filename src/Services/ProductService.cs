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

    public async Task<IEnumerable<Product>> GetAllActiveProductsAsync()
    {
        return await _db.Products
            .Include(p => p.Category)
            .Where(p => p.isDeleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllActiveCategoriesAsync()
    {
        return await _db.Categories
            .Where(p => p.isDeleted == false)
            .ToListAsync();
    }
}