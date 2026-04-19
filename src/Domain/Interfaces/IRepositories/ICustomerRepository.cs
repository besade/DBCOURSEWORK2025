using Shop.Domain.Models;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Domain.Interfaces.IRepositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Customer?> GetByEmailAsync(Email email, CancellationToken ct = default);
        Task<Customer?> GetByPhoneAsync(PhoneNumber phone, CancellationToken ct = default);
        Task<bool> AdminExistsAsync(CancellationToken ct = default);
        Task AddAsync(Customer customer, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
