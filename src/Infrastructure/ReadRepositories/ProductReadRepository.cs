using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Infrastructure.Queries
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int productId, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductResponseDto(
                    p.ProductId,
                    p.ProductName,
                    p.ProductCountry,
                    p.Weight,
                    p.Price,
                    p.Picture,
                    p.Category.CategoryName
                ))
                .FirstOrDefaultAsync(ct);

        }

        public async Task<ProductsListResponseDto> GetAllProductsAsync(CancellationToken ct)
        {
            var products = await _context.Products
            .AsNoTracking()
            .Select(p => new ProductShortResponseDto
            (
                p.ProductId,
                p.ProductName,
                p.Price,
                p.Picture,
                p.Category.CategoryName
            ))
            .ToListAsync(ct);

            return new ProductsListResponseDto(products);
        }

        public async Task<(IEnumerable<PagedProductsResponseDto> PagedProducts, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.IsDeleted == false);

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            int totalCount = await query.CountAsync(ct);

            var products = await query
                .OrderBy(p => p.ProductId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PagedProductsResponseDto(p.ProductId, p.ProductName, p.Price, p.Picture))
                .ToListAsync(ct);

            return (products, totalCount);
        }
    }
}
