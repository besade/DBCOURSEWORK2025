using Shop.Application.DTOs;

namespace Shop.Application.IQueries
{
    public interface IOrderReadRepository
    {
        Task<List<OrderResponseDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
        Task<List<OrderResponseDto>> GetAllAsync(CancellationToken ct = default);
    }
}
