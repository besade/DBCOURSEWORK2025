using MediatR;

namespace Shop.Application.Commands.Product.DeleteProduct
{
    public record DeleteProductCommand(int ProductId) : IRequest;
}
