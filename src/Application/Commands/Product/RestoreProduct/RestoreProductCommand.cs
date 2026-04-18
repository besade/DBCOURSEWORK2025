using MediatR;

namespace Shop.Application.Commands.Product.RestoreProduct
{
    public record RestoreProductCommand(int ProductId) : IRequest;
}
