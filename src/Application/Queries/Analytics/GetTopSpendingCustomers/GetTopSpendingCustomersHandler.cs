using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Analytics.GetTopSpendingCustomers
{
    public class GetTopSpendingCustomersQueryHandler : IRequestHandler<GetTopSpendingCustomersQuery, IEnumerable<CustomerSpendingResponseDto>>
    {
        private readonly IAnalyticsReadRepository _analyticsReadRepository;

        public GetTopSpendingCustomersQueryHandler(IAnalyticsReadRepository analyticsReadRepository)
        {
            _analyticsReadRepository = analyticsReadRepository;
        }

        public async Task<IEnumerable<CustomerSpendingResponseDto>> Handle(GetTopSpendingCustomersQuery query, CancellationToken ct)
        {
            return await _analyticsReadRepository.GetTopSpendingCustomersAsync(ct);
        }
    }
}
