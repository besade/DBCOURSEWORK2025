using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface IAddressReadRepository
    {
        Task<CustomerAddressesResponseDto> GetActiveByCustomerIdAsync(int customerId, CancellationToken ct = default);
        Task<CustomerAddressesResponseDto> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
    }
}
