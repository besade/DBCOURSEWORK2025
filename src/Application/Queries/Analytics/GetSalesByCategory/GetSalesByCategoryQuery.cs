using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Analytics.GetSalesByCategory
{
    public record GetSalesByCategoryQuery() : IRequest<IEnumerable<CategorySalesResponseDto>>;
}
