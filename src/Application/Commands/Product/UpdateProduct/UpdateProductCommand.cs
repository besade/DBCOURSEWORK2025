using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Product.UpdateProduct
{
    public record UpdateProductCommand(int ProductId, ProductRequestDto Dto) : IRequest;
}
