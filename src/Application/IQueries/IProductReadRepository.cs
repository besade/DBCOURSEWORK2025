using Shop.Application.DTOs;

namespace Shop.Application.Queries
{
    public interface IProductReadRepository
    {
        Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<ProductShortResponseDto>> GetAllProductsAsync(CancellationToken ct = default);
        Task<(IEnumerable<PagedProductsResponseDto> PagedProducts, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
