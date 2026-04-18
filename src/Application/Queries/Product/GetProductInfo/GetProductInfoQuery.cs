using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Product.GetProductInfo
{
    public record GetProductInfoQuery(int ProductId) : IRequest<ProductResponseDto>;
}
