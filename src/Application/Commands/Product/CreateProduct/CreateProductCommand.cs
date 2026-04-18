using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Product.CreateProduct
{
    public record CreateProductCommand(ProductRequestDto Dto) : IRequest;
}
