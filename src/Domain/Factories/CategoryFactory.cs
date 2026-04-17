using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;

namespace Shop.Domain.Factories
{
    public class CategoryFactory : ICategoryFactory
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryFactory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateAsync(string name)
        {
            if (await _categoryRepository.GetByNameAsync(name) != null)
                throw new DomainValidationException("Категорія з вказаним ім'ям вже існує.");

            return new Category(name);
        }
    }
}
