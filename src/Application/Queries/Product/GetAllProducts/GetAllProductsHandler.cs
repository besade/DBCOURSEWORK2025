using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ProductsListResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<ProductsListResponseDto> Handle(GetAllProductsQuery query, CancellationToken ct)
        {
            return await _productReadRepository.GetAllProductsAsync(ct);
        }
    }
}
