using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface ICartReadRepository
    {
        Task<CartResponseDto> GetCartDetailsAsync(int customerId, CancellationToken ct = default);
        Task<int> GetCountAsync(int customerId, CancellationToken ct = default);
    }
}
