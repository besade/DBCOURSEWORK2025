using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Analytics.GetSalesByCategory
{
    public class GetSalesByCategoryQueryHandler : IRequestHandler<GetSalesByCategoryQuery, IEnumerable<CategorySalesResponseDto>>
    {
        private readonly IAnalyticsReadRepository _analyticsReadRepository;

        public GetSalesByCategoryQueryHandler(IAnalyticsReadRepository analyticsReadRepository)
        {
            _analyticsReadRepository = analyticsReadRepository;
        }

        public async Task<IEnumerable<CategorySalesResponseDto>> Handle(GetSalesByCategoryQuery query,CancellationToken ct)
        {
            return await _analyticsReadRepository.GetSalesByCategoryAsync(ct);
        }
    }
}
