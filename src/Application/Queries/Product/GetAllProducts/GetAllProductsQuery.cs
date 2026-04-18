using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Product.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<ProductsListResponseDto>;
}
