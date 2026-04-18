using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Category.UpdateCategoryName
{
    public class UpdateCategoryNameCommandHandler : IRequestHandler<UpdateCategoryNameCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryNameCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(UpdateCategoryNameCommand command, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId, ct);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), command.CategoryId);
            }

            if (category.CategoryName != command.NewName)
            {
                var existing = await _categoryRepository.GetByNameAsync(command.NewName, ct);
                if (existing != null)
                    throw new DomainValidationException("Вказана назва вже зайнята іншою категорією.");
            }

            category.UpdateName(command.NewName);

            await _categoryRepository.SaveChangesAsync(ct);
        }
    }
}
