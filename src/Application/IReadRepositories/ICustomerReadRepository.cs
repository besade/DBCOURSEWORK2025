using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface ICustomerReadRepository
    {
        Task<CustomerProfileResponseDto?> GetProfileByIdAsync(int customerId, CancellationToken ct = default);
    }
}
