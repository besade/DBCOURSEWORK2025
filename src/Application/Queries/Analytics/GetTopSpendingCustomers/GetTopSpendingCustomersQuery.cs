using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Analytics.GetTopSpendingCustomers
{
    public record GetTopSpendingCustomersQuery() : IRequest<IEnumerable<CustomerSpendingResponseDto>>;
}
