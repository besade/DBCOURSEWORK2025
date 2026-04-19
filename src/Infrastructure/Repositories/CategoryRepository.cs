using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Categories.FindAsync(id, ct);
        }

        public async Task<Category?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name, ct);
        }

        public async Task AddAsync(Category category, CancellationToken ct = default)
        {
            await _context.Categories.AddAsync(category, ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
