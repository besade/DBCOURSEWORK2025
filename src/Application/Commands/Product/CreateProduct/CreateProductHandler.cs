using MediatR;
using Shop.Domain.Interfaces.IRepositories;
using ProductEntity = Shop.Domain.Models.Product;


namespace Shop.Application.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(CreateProductCommand command, CancellationToken ct)
        {
            var dto = command.Dto;

            var product = new ProductEntity(dto.ProductName, dto.ProductCountry, dto.Weight, dto.Price, dto.StockQuantity, dto.CategoryId, dto.Picture);

            await _productRepository.AddAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);
        }
    }
}
