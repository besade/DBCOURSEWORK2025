using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Product.GetPagedProducts
{
    public record GetPagedProductsQuery(int? CategoryId, int PageNumber, int PageSize = 6) : IRequest<PagedProductsListResponseDto>;
}
