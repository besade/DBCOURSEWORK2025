using Moq;
using Shop.Application.Commands.Cart.AddItem;
using Shop.Application.Commands.Cart.IncreaseCartItemQuantity;
using Shop.Application.Commands.Cart.RemoveItemFromCart;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Cart;

public class CartCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();

    [Fact]
    public async Task AddItem_CartNotFound_ThrowsNotFoundException()
    {
        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync((Shop.Domain.Models.Cart?)null);

        var handler = new AddItemToCartCommandHandler(_cartRepo.Object, _productRepo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new AddItemToCartCommand(1, 10), default));
    }

    [Fact]
    public async Task AddItem_ProductNotFound_ThrowsNotFoundException()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _productRepo.Setup(r => r.GetByIdAsync(10, default))
                    .ReturnsAsync((Shop.Domain.Models.Product?)null);

        var handler = new AddItemToCartCommandHandler(_cartRepo.Object, _productRepo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new AddItemToCartCommand(1, 10), default));
    }

    [Fact]
    public async Task AddItem_OutOfStock_ThrowsDomainValidationException()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        var product = DomainObjectBuilder.CreateProduct(stock: 0);

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _productRepo.Setup(r => r.GetByIdAsync(10, default))
                    .ReturnsAsync(product);

        var handler = new AddItemToCartCommandHandler(_cartRepo.Object, _productRepo.Object);
        await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(new AddItemToCartCommand(1, 10), default));
    }

    [Fact]
    public async Task AddItem_ValidRequest_AddsItemAndSaves()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        var product = DomainObjectBuilder.CreateProduct(stock: 5);

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _productRepo.Setup(r => r.GetByIdAsync(10, default))
                    .ReturnsAsync(product);
        _cartRepo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new AddItemToCartCommandHandler(_cartRepo.Object, _productRepo.Object);
        await handler.Handle(new AddItemToCartCommand(1, 10), default);

        Assert.Single(customer.Cart.CartItems);
        _cartRepo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Increase_CartNotFound_ThrowsNotFoundException()
    {
        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync((Shop.Domain.Models.Cart?)null);

        var handler = new IncreaseCartItemQuantityCommandHandler(_cartRepo.Object, _productRepo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new IncreaseCartItemQuantityCommand(1, 10), default));
    }

    [Fact]
    public async Task Increase_QuantityExceedsStock_ThrowsDomainValidationException()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        customer.Cart.AddItem(10);
        var product = DomainObjectBuilder.CreateProduct(stock: 1);

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _productRepo.Setup(r => r.GetByIdAsync(10, default))
                    .ReturnsAsync(product);

        var handler = new IncreaseCartItemQuantityCommandHandler(_cartRepo.Object, _productRepo.Object);
        await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(new IncreaseCartItemQuantityCommand(1, 10), default));
    }

    [Fact]
    public async Task Increase_WithinStock_IncreasesQuantity()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        customer.Cart.AddItem(10);
        var product = DomainObjectBuilder.CreateProduct(stock: 10);

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _productRepo.Setup(r => r.GetByIdAsync(10, default))
                    .ReturnsAsync(product);
        _cartRepo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new IncreaseCartItemQuantityCommandHandler(_cartRepo.Object, _productRepo.Object);
        await handler.Handle(new IncreaseCartItemQuantityCommand(1, 10), default);

        Assert.Equal(2, customer.Cart.CartItems.First().Quantity);
    }

    [Fact]
    public async Task Remove_CartNotFound_ThrowsNotFoundException()
    {
        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync((Shop.Domain.Models.Cart?)null);

        var handler = new RemoveItemFromCartCommandHandler(_cartRepo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new RemoveItemFromCartCommand(1, 10), default));
    }

    [Fact]
    public async Task Remove_ValidItem_RemovesFromCart()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        customer.Cart.AddItem(10);

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync(customer.Cart);
        _cartRepo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new RemoveItemFromCartCommandHandler(_cartRepo.Object);
        await handler.Handle(new RemoveItemFromCartCommand(1, 10), default);

        Assert.Empty(customer.Cart.CartItems);
    }
}