using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IRepositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
        void Update(Cart cart);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
