using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface IProductReadRepository
    {
        Task<ProductResponseDto?> GetProductByIdAsync(int productId, CancellationToken ct = default);
        Task<ProductsListResponseDto> GetAllProductsAsync(CancellationToken ct = default);
        Task<(IEnumerable<PagedProductsResponseDto> PagedProducts, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
