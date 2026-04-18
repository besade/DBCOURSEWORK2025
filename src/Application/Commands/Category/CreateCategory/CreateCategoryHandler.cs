using MediatR;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryFactory _categoryFactory;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ICategoryFactory categoryFactory)
        {
            _categoryRepository = categoryRepository;
            _categoryFactory = categoryFactory;
        }

        public async Task Handle(CreateCategoryCommand command, CancellationToken ct)
        {
            var category = await _categoryFactory.CreateAsync(command.Dto.CategoryName);

            await _categoryRepository.AddAsync(category, ct);
            await _categoryRepository.SaveChangesAsync(ct);
        }
    }
}
