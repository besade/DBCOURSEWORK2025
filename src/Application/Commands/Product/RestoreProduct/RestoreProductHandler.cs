using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Product.RestoreProduct
{
    public class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public RestoreProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(RestoreProductCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(command.ProductId, ct);

            if (product == null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            product.Restore();

            await _productRepository.SaveChangesAsync(ct);
        }
    }
}
