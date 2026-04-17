using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryRequestDto dto, CancellationToken ct);
        Task UpdateNameAsync(int id, string newName, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task RestoreAsync(int id, CancellationToken ct);
        Task <IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync();
    }
}
