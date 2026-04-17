using Shop.Application.DTOs;

namespace Shop.Application.IQueries
{
    public interface ICartReadRepository
    {
        Task<CartResponseDto?> GetCartDetailsAsync(int customerId, CancellationToken ct = default);
        Task<int> GetCountAsync(int customerId, CancellationToken ct = default);
    }
}
