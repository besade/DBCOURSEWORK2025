using Shop.Application.DTOs;

namespace Shop.Application.IQueries
{
    public interface ICategoryReadRepository
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<IEnumerable<CategoryResponseDto>> GetAllActiveCategoriesAsync(CancellationToken ct = default);
    }
}
