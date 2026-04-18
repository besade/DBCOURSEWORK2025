using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Product.GetPagedProducts
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, PagedProductsListResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetPagedProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<PagedProductsListResponseDto> Handle(GetPagedProductsQuery query, CancellationToken ct)
        {
            if (query.CategoryId.HasValue && query.CategoryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(query.CategoryId), "ID категорії не може бути від'ємним або нулем.");
            }

            if (query.PageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(query.PageNumber), "Номер сторінки не може бути від'ємним або нулем.");
            }

            var (products, totalCount) = await _productReadRepository.GetPagedProductsAsync(query.CategoryId, query.PageNumber, query.PageSize, ct);

            return new PagedProductsListResponseDto(products, totalCount, query.PageNumber);
        }
    }
}
