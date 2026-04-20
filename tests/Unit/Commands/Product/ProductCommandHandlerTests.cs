using Moq;
using Shop.Application.Commands.Product.CreateProduct;
using Shop.Application.Commands.Product.DeleteProduct;
using Shop.Application.Commands.Product.UpdateProduct;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Product;

public class ProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _repo = new();

    private static ProductRequestDto ValidDto(int stock = 5)
        => new("Updated", "Ukraine", 0.5m, 149.99m, stock, 1, new byte[16]);

    [Fact]
    public async Task Create_ValidDto_AddsAndSaves()
    {
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new CreateProductCommandHandler(_repo.Object);
        await handler.Handle(new CreateProductCommand(ValidDto()), default);

        _repo.Verify(r => r.AddAsync(It.IsAny<Shop.Domain.Models.Product>(), default), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Update_ProductNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Product?)null);

        var handler = new UpdateProductCommandHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new UpdateProductCommand(99, ValidDto()), default));
    }

    [Fact]
    public async Task Update_ValidData_UpdatesAllFieldsAndSaves()
    {
        var product = DomainObjectBuilder.CreateProduct(stock: 3, price: 50m);
        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(product);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new UpdateProductCommandHandler(_repo.Object);
        await handler.Handle(new UpdateProductCommand(1, ValidDto(stock: 20)), default);

        Assert.Equal("Updated", product.ProductName);
        Assert.Equal(149.99m, product.Price);
        Assert.Equal(20, product.StockQuantity);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Update_EmptyPicture_DoesNotUpdatePicture()
    {
        var product = DomainObjectBuilder.CreateProduct();
        var originalPicture = product.Picture;

        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(product);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var dto = new ProductRequestDto("Updated", "Ukraine", 0.5m, 99m, 5, 1, Array.Empty<byte>());
        var handler = new UpdateProductCommandHandler(_repo.Object);
        await handler.Handle(new UpdateProductCommand(1, dto), default);

        Assert.Equal(originalPicture, product.Picture);
    }

    [Fact]
    public async Task Delete_ValidProduct_MarksDeletedAndSaves()
    {
        var product = DomainObjectBuilder.CreateProduct();
        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(product);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new DeleteProductCommandHandler(_repo.Object);
        await handler.Handle(new DeleteProductCommand(1), default);

        Assert.True(product.IsDeleted);
    }
}