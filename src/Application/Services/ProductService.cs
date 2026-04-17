using Shop.Application.Interfaces;
using Shop.Application.Queries;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;

    public ProductService(IProductRepository productRepository, IProductReadRepository productReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
    }

    // Create product
    public async Task CreateAsync(ProductRequestDto dto, CancellationToken ct)
    {
        var product = new Product(dto.ProductName, dto.ProductCountry, dto.Weight, dto.Price, dto.StockQuantity, dto.CategoryId, dto.Picture);

        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);
    }


    // Update product information
    public async Task UpdateProductAsync(int productId, ProductRequestDto dto, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);

        if (product == null) 
            throw new DomainNotFoundException(nameof(Product), productId);

        product.UpdateDetails(dto.ProductName, dto.ProductCountry, dto.Weight, dto.CategoryId);
        product.UpdatePrice(dto.Price);
        product.UpdateStock(dto.StockQuantity);

        if (dto.Picture != null && dto.Picture.Length > 0)
        {
            product.UpdatePicture(dto.Picture);
        }

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(ct);
    }

    // Delete product
    public async Task DeleteProductAsync(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null) throw new DomainNotFoundException(nameof(Product), productId);

        product.MarkAsDeleted();
        _productRepository.Update(product);

        await _productRepository.SaveChangesAsync(ct);
    }

    // Restore product
    public async Task RestoreProductAsync(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null) throw new DomainNotFoundException(nameof(Product), productId);

        product.Restore();
        _productRepository.Update(product);

        await _productRepository.SaveChangesAsync(ct);
    }

    // Get product info
    public async Task<ProductResponseDto> GetProductInfoAsync(int productId, CancellationToken ct)
    {
        var productDto = await _productReadRepository.GetProductByIdAsync(productId, ct);

        if (productDto == null)
            throw new DomainNotFoundException(nameof(Product), productId);

        return productDto;
    }

    // Get all products
    public async Task<IEnumerable<ProductShortResponseDto>> GetAllProductsAsync(CancellationToken ct)
    {
        return await _productReadRepository.GetAllProductsAsync(ct);
    }

    // Get paged products
    public async Task<(IEnumerable<PagedProductsResponseDto> PagedProducts, int TotalCount)> GetPagedProductsAsync(int? categoryId, int pageNumber, CancellationToken ct)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(categoryId), "ID категорії не може бути від'ємним або нулем.");
        }

        if (pageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер сторінки не може бути від'ємним або нулем.");
        }

        var pageSize = 6;

        return await _productReadRepository.GetPagedProductsAsync(categoryId, pageNumber, pageSize, ct);
    }
}