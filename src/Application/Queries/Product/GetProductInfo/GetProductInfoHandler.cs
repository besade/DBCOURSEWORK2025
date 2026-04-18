using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Product.GetProductInfo
{
    public class GetProductInfoQueryHandler : IRequestHandler<GetProductInfoQuery, ProductResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetProductInfoQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<ProductResponseDto> Handle(GetProductInfoQuery query, CancellationToken ct)
        {
            var productDto = await _productReadRepository.GetProductByIdAsync(query.ProductId, ct);

            if (productDto == null)
                throw new NotFoundException(nameof(Product), query.ProductId);

            return productDto;
        }
    }
}
