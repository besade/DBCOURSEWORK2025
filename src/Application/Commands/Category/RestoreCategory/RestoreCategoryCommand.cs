using MediatR;

namespace Shop.Application.Commands.Category.RestoreCategory
{
    public record RestoreCategoryCommand(int CategoryId) : IRequest;
}
