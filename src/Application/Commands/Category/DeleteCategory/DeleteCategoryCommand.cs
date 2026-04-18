using MediatR;

namespace Shop.Application.Commands.Category.DeleteCategory
{
    public record DeleteCategoryCommand(int CategoryId) : IRequest;
}
