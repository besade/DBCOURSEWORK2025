using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Application.IQueries;


public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsReadRepository _analyticsReadRepository;

    public AnalyticsService(IAnalyticsReadRepository analyticsReadRepository)
    {
        _analyticsReadRepository = analyticsReadRepository;
    }

    // Get sales by categories
    public async Task<IEnumerable<CategorySalesResponseDto>> GetSalesByCategoryAsync(CancellationToken ct)
    {
        return await _analyticsReadRepository.GetSalesByCategoryAsync(ct);
    }

    // Get customers top ordered by spent money
    public async Task<IEnumerable<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct)
    {
        return await _analyticsReadRepository.GetTopSpendingCustomersAsync(ct);
    }
}