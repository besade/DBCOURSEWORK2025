using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IRepositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
