using MediatR;

namespace Shop.Application.Commands.Category.UpdateCategoryName
{
    public record UpdateCategoryNameCommand(int CategoryId, string NewName) : IRequest;
}
