using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IRepositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Category?> GetByNameAsync(string name, CancellationToken ct = default);
        Task AddAsync(Category category, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
