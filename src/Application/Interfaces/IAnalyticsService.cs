using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<CategorySalesResponseDto>> GetSalesByCategoryAsync(CancellationToken ct);
        Task<IEnumerable<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct);
    }
}
