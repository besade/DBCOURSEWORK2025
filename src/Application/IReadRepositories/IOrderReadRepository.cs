using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface IOrderReadRepository
    {
        Task<OrdersListResponseDto> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
        Task<OrdersListResponseDto> GetAllAsync(CancellationToken ct = default);
    }
}
