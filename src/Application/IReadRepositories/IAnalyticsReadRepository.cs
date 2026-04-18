using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface IAnalyticsReadRepository
    {
        Task<List<CategorySalesResponseDto>> GetSalesByCategoryAsync(CancellationToken ct = default);
        Task<List<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct = default);
    }
}
