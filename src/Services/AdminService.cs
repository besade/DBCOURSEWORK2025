using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.DTOs;
using Shop.Models;
using Shop.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _db;

    public AdminService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _db.Products.FindAsync(id);
    }

    public async Task CreateProductAsync(ProductDto dto)
    {
        byte[] pictureBytes = Array.Empty<byte>();

        if (dto.PictureFile != null)
        {
            using var memoryStream = new MemoryStream();

            await dto.PictureFile.CopyToAsync(memoryStream);

            pictureBytes = memoryStream.ToArray();
        }

        var product = new Product
        {
            ProductName = dto.ProductName,
            CategoryId = dto.CategoryId,
            ProductCountry = dto.ProductCountry,
            Weight = dto.Weight,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            Picture = pictureBytes
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(int id, ProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);

        if (product != null)
        {
            product.ProductName = dto.ProductName;
            product.CategoryId = dto.CategoryId;
            product.ProductCountry = dto.ProductCountry;
            product.Weight = dto.Weight;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            if (dto.PictureFile != null)
            {
                using var memoryStream = new MemoryStream();
                await dto.PictureFile.CopyToAsync(memoryStream);
                product.Picture = memoryStream.ToArray();
            }

            await _db.SaveChangesAsync();
        }
        ;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            product.isDeleted = true;

            await _db.SaveChangesAsync();
        }
    }

    public async Task RestoreProductAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            product.isDeleted = false;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _db.Categories.FindAsync(id);
    }

    public async Task CreateCategoryAsync(CategoryDto dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(int id, CategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category != null)
        {
            category.CategoryName = dto.CategoryName;

            await _db.SaveChangesAsync();
        }
        ;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            category.isDeleted = true;

            await _db.SaveChangesAsync();
        }
    }

    public async Task RestoreCategoryAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            category.isDeleted = false;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CategorySalesDto>> GetSalesByCategoryAsync()
    {
        return await _db.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .Include(oi => oi.Orders)
            .Where(oi => oi.Orders.OrderStatus == Status.Success)
            .GroupBy(oi => oi.Product.Category.CategoryName)
            .Select(g => new CategorySalesDto
            {
                CategoryName = g.Key,
                TotalQuantitySold = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToListAsync();
    }
    public async Task<IEnumerable<CustomerSpendingDto>> GetTopSpendingCustomersAsync()
    {
        return await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.OrderStatus == Status.Success)
            .GroupBy(o => o.Customer.Email)
            .Select(g => new CustomerSpendingDto
            {
                Email = g.Key,
                OrdersCount = g.Count(),
                TotalSpent = g.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice))
            })
            .Where(x => x.TotalSpent > 1000)
            .OrderByDescending(x => x.TotalSpent)
            .ToListAsync();
    }
}