using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Infrastructure.Queries
{
    public class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoriesListResponseDto> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryResponseDto(c.CategoryId, c.CategoryName))
                .ToListAsync(ct);

            return new CategoriesListResponseDto(categories);
        }

        public async Task<CategoriesListResponseDto> GetAllActiveCategoriesAsync(CancellationToken ct = default)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryResponseDto(c.CategoryId, c.CategoryName))
                .ToListAsync(ct);

            return new CategoriesListResponseDto(categories);
        }
    }
}
