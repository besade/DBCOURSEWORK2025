using Microsoft.EntityFrameworkCore;
using Shop.Application.Queries;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;

namespace Shop.Infrastructure.Queries
{
    public class ProductReadRepository : IProductReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductId == id)
                .Select(p => new ProductResponseDto(
                    p.ProductName,
                    p.ProductCountry,
                    p.Weight,
                    p.Price,
                    p.Picture,
                    p.Category.CategoryName
                ))
                .FirstOrDefaultAsync(ct);

        }

        public async Task<IEnumerable<ProductShortResponseDto>> GetAllProductsAsync(CancellationToken ct)
        {
            return await _context.Products
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
