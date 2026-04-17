using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IRepositories
{
    public interface IAddressRepository
    {
        Task<Address?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Address>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
        Task AddAsync(Address address, CancellationToken ct = default);
        void Update(Address address);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
