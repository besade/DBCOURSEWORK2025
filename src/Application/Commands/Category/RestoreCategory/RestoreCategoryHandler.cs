using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Category.RestoreCategory
{
    public class RestoreCategoryCommandHandler : IRequestHandler<RestoreCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public RestoreCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(RestoreCategoryCommand command, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId, ct);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), command.CategoryId);
            }

            category.Restore();

            await _categoryRepository.SaveChangesAsync(ct);
        }
    }
}
