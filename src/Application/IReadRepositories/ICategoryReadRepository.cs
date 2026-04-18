using Shop.Application.DTOs;

namespace Shop.Application.IReadRepositories
{
    public interface ICategoryReadRepository
    {
        Task<CategoriesListResponseDto> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<CategoriesListResponseDto> GetAllActiveCategoriesAsync(CancellationToken ct = default);
    }
}
