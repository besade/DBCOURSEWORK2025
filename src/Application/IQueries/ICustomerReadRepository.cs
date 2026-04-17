using Shop.Application.DTOs;

namespace Shop.Application.IQueries
{
    public interface ICustomerReadRepository
    {
        Task<CustomerProfileResponseDto?> GetProfileByIdAsync(int id, CancellationToken ct = default);
    }
}
