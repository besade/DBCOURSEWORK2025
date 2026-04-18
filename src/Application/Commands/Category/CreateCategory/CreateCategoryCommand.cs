using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Category.CreateCategory
{
    public record CreateCategoryCommand(CategoryRequestDto Dto) : IRequest;
}
