using Moq;
using Shop.Application.Commands.Address.AddAddress;
using Shop.Application.Commands.Address.DeleteAddress;
using Shop.Application.Commands.Address.SetDefaultAddress;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Address;

public class AddressCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _repo = new();

    [Fact]
    public async Task Add_IsDefaultFalse_AddsWithoutFetchingOthers()
    {
        var dto = new AddressRequestDto("Ukraine", "Kyiv", "Street 1", false);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new AddAddressHandler(_repo.Object);
        var id = await handler.Handle(new AddAddressCommand(1, dto), default);

        _repo.Verify(r => r.GetByCustomerIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _repo.Verify(r => r.AddAsync(It.IsAny<Shop.Domain.Models.Address>(), default), Times.Once);
        Assert.True(id > 0 || id == 0);
    }

    [Fact]
    public async Task Add_IsDefaultTrue_UnsetsExistingDefaultFirst()
    {
        var existingDefault = DomainObjectBuilder.CreateAddress(customerId: 1, isDefault: true);
        _repo.Setup(r => r.GetByCustomerIdAsync(1, default))
             .ReturnsAsync(new[] { existingDefault });
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var dto = new AddressRequestDto("Ukraine", "Lviv", "Avenue 2", true);
        var handler = new AddAddressHandler(_repo.Object);
        await handler.Handle(new AddAddressCommand(1, dto), default);

        Assert.False(existingDefault.IsDefault);
    }

    [Fact]
    public async Task Delete_AddressNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Address?)null);

        var handler = new DeleteAddressHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new DeleteAddressCommand(1, 99), default));
    }

    [Fact]
    public async Task Delete_WrongCustomer_ThrowsNotFoundException()
    {
        var address = DomainObjectBuilder.CreateAddress(customerId: 2);
        _repo.Setup(r => r.GetByIdAsync(5, default)).ReturnsAsync(address);

        var handler = new DeleteAddressHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new DeleteAddressCommand(CustomerId: 1, AddressId: 5), default));
    }

    [Fact]
    public async Task Delete_ValidAddress_MarksAsDeleted()
    {
        var address = DomainObjectBuilder.CreateAddress(customerId: 1);
        _repo.Setup(r => r.GetByIdAsync(5, default)).ReturnsAsync(address);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new DeleteAddressHandler(_repo.Object);
        await handler.Handle(new DeleteAddressCommand(1, 5), default);

        Assert.True(address.IsDeleted);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SetDefault_AddressNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Address?)null);

        var handler = new SetDefaultAddressHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new SetDefaultAddressCommand(1, 99), default));
    }

    [Fact]
    public async Task SetDefault_ValidAddress_UnsetsOthersAndSetsTarget()
    {
        var oldDefault = DomainObjectBuilder.CreateAddress(customerId: 1, isDefault: true);
        var target = DomainObjectBuilder.CreateAddress(customerId: 1, isDefault: false);

        _repo.Setup(r => r.GetByIdAsync(2, default)).ReturnsAsync(target);
        _repo.Setup(r => r.GetByCustomerIdAsync(1, default))
             .ReturnsAsync(new[] { oldDefault, target });
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new SetDefaultAddressHandler(_repo.Object);
        await handler.Handle(new SetDefaultAddressCommand(1, 2), default);

        Assert.False(oldDefault.IsDefault);
        Assert.True(target.IsDefault);
    }
}