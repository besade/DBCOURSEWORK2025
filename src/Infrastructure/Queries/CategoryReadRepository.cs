using Microsoft.EntityFrameworkCore;
using Shop.Application.IQueries;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;

namespace Shop.Infrastructure.Queries
{
    public class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryResponseDto(c.CategoryId, c.CategoryName))
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllActiveCategoriesAsync(CancellationToken ct = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryResponseDto(c.CategoryId, c.CategoryName))
                .ToListAsync(ct);
        }
    }
}
