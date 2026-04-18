using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateProductCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(command.ProductId, ct);

            if (product == null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            var dto = command.Dto;

            product.UpdateDetails(dto.ProductName, dto.ProductCountry, dto.Weight, dto.CategoryId);
            product.UpdatePrice(dto.Price);
            product.UpdateStock(dto.StockQuantity);

            if (dto.Picture != null && dto.Picture.Length > 0)
            {
                product.UpdatePicture(dto.Picture);
            }

            await _productRepository.SaveChangesAsync(ct);
        }
    }
}
