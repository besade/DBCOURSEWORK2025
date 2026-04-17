using Shop.Application.Interfaces;
using Shop.Application.IQueries;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryFactory _categoryFactory;

        public CategoryService(ICategoryRepository categoryRepository, ICategoryFactory categoryFactory, ICategoryReadRepository categoryReadRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryFactory = categoryFactory;
            _categoryReadRepository = categoryReadRepository;
        }

        // Create category
        public async Task CreateAsync(CategoryRequestDto dto, CancellationToken ct)
        {
            var category = await _categoryFactory.CreateAsync(dto.CategoryName);

            await _categoryRepository.AddAsync(category, ct);
            await _categoryRepository.SaveChangesAsync(ct);
        }

        // Update category name
        public async Task UpdateNameAsync(int id, string newName, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct);

            if (category == null)
            {
                throw new DomainNotFoundException(nameof(Category), id);
            }

            if (category.CategoryName != newName)
            {
                var existing = await _categoryRepository.GetByNameAsync(newName, ct);
                if (existing != null)
                    throw new DomainValidationException("Вказана назва вже зайнята іншою категорією.");
            }

            category.UpdateName(newName);
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync(ct);
        }

        // Delete category
        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct);

            if (category == null)
            {
                throw new DomainNotFoundException(nameof(Category), id);
            }

            if (category.IsDeleted == true)
            {
                throw new DomainValidationException("Вказана категорія вже видалена.");
            }

            category.MarkAsDeleted();
            _categoryRepository.Update(category);

            await _categoryRepository.SaveChangesAsync(ct);
        }

        // Restore category
        public async Task RestoreAsync(int id, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct);

            if (category == null)
            {
                throw new DomainNotFoundException(nameof(Category), id);
            }

            if (category.IsDeleted == false)
            {
                throw new DomainValidationException("Вказана категорія не видалена.");
            }

            category.Restore();
            _categoryRepository.Update(category);

            await _categoryRepository.SaveChangesAsync(ct);
        }

        // Get all categories
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            return await _categoryReadRepository.GetAllCategoriesAsync();
        }

        // Get all active categories
        public async Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync()
        {
            return await _categoryReadRepository.GetAllActiveCategoriesAsync();
        }
    }
}
