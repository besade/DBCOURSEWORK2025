using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductRequestDto dto, CancellationToken ct);
        Task UpdateProductAsync(int productId, ProductRequestDto dto, CancellationToken ct);
        Task DeleteProductAsync(int productId, CancellationToken ct);
        Task RestoreProductAsync(int productId, CancellationToken ct);
        Task<ProductResponseDto> GetProductInfoAsync(int productId, CancellationToken ct);
        Task<IEnumerable<ProductShortResponseDto>> GetAllProductsAsync(CancellationToken ct);
        Task<(IEnumerable<PagedProductsResponseDto> PagedProducts, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, CancellationToken ct);
    }
}