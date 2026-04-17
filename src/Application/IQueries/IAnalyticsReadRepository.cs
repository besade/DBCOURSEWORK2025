using Shop.Application.DTOs;

namespace Shop.Application.IQueries
{
    public interface IAnalyticsReadRepository
    {
        Task<List<CategorySalesResponseDto>> GetSalesByCategoryAsync(CancellationToken ct = default);
        Task<List<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct = default);
    }
}
