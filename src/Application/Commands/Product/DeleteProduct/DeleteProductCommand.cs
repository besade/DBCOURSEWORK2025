using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Product.DeleteProduct
{
    public record DeleteProductCommand(int ProductId) : IRequest;
}
