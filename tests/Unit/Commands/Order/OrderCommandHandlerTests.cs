using Moq;
using Shop.Application.Commands.Order.CreateOrder;
using Shop.Application.Commands.Order.UpdateOrderStatus;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Order;

public class OrderCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepo = new();
    private readonly Mock<IOrderRepository> _orderRepo = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IOrderFactory> _factory = new();
    private readonly Mock<IOrderRepository> _orderRepoStatus = new();

    [Fact]
    public async Task Create_CartNotFound_ThrowsNotFoundException()
    {
        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default))
                 .ReturnsAsync((Shop.Domain.Models.Cart?)null);

        var handler = new CreateOrderCommandHandler(
            _cartRepo.Object, _orderRepo.Object, _productRepo.Object, _factory.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(
                new CreateOrderCommand(1, new OrderRequestDto(1, DeliveryType.NovaPoshta, null, null, true)),
                default));
    }

    [Fact]
    public async Task Create_ProductInOrderNotFound_ThrowsNotFoundException()
    {
        var customer = DomainObjectBuilder.CreateCustomer();

        var order = new Shop.Domain.Models.Order(1, 1, "Test", "User", DeliveryType.NovaPoshta, true);
        order.OrderItems.Add(new OrderItem(99, 100m, 1));

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default)).ReturnsAsync(customer.Cart);
        _factory.Setup(f => f.CreateAsync(
            It.IsAny<Shop.Domain.Models.Cart>(),
            It.IsAny<int>(), It.IsAny<DeliveryType>(), It.IsAny<bool>(),
            It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(order);
        _productRepo.Setup(r => r.GetByIdAsync(99, default))
                    .ReturnsAsync((Shop.Domain.Models.Product?)null);

        var handler = new CreateOrderCommandHandler(
            _cartRepo.Object, _orderRepo.Object, _productRepo.Object, _factory.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(
                new CreateOrderCommand(1, new OrderRequestDto(1, DeliveryType.NovaPoshta, null, null, true)),
                default));
    }

    [Fact]
    public async Task Create_ValidRequest_SavesOrderAndClearsCart()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        customer.Cart.AddItem(5);

        var product = DomainObjectBuilder.CreateProduct(stock: 10);
        var order = new Shop.Domain.Models.Order(1, 1, "Test", "User", DeliveryType.NovaPoshta, true);
        order.OrderItems.Add(new OrderItem(5, 100m, 2));

        _cartRepo.Setup(r => r.GetByCustomerIdAsync(1, default)).ReturnsAsync(customer.Cart);
        _factory.Setup(f => f.CreateAsync(
            It.IsAny<Shop.Domain.Models.Cart>(),
            It.IsAny<int>(), It.IsAny<DeliveryType>(), It.IsAny<bool>(),
            It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(order);
        _productRepo.Setup(r => r.GetByIdAsync(5, default)).ReturnsAsync(product);
        _orderRepo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new CreateOrderCommandHandler(
            _cartRepo.Object, _orderRepo.Object, _productRepo.Object, _factory.Object);

        await handler.Handle(
            new CreateOrderCommand(1, new OrderRequestDto(1, DeliveryType.NovaPoshta, null, null, true)),
            default);

        _orderRepo.Verify(r => r.AddAsync(order, default), Times.Once);
        Assert.Empty(customer.Cart.CartItems);
    }

    [Fact]
    public async Task UpdateStatus_OrderNotFound_ThrowsNotFoundException()
    {
        _orderRepoStatus.Setup(r => r.GetByIdAsync(99, default))
                        .ReturnsAsync((Shop.Domain.Models.Order?)null);

        var handler = new UpdateOrderStatusCommandHandler(_orderRepoStatus.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new UpdateOrderStatusCommand(99, Status.Success), default));
    }

    [Fact]
    public async Task UpdateStatus_InvalidTransition_ThrowsDomainValidationException()
    {
        var order = new Shop.Domain.Models.Order(1, 1, "Test", "User", DeliveryType.NovaPoshta, true);
        order.ChangeStatus(Status.Failed);

        _orderRepoStatus.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(order);

        var handler = new UpdateOrderStatusCommandHandler(_orderRepoStatus.Object);
        await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(new UpdateOrderStatusCommand(1, Status.Success), default));
    }

    [Fact]
    public async Task UpdateStatus_ValidTransition_ChangesStatusAndSaves()
    {
        var order = new Shop.Domain.Models.Order(1, 1, "Test", "User", DeliveryType.NovaPoshta, true);

        _orderRepoStatus.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(order);
        _orderRepoStatus.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new UpdateOrderStatusCommandHandler(_orderRepoStatus.Object);
        await handler.Handle(new UpdateOrderStatusCommand(1, Status.Success), default);

        Assert.Equal(Status.Success, order.OrderStatus);
        _orderRepoStatus.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}