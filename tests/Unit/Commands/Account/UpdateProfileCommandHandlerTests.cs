using Moq;
using Shop.Application.Commands.Account.UpdateProfile;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Tests.Unit.Commands.Account;

public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repo = new();

    private UpdateProfileCommandHandler CreateHandler()
        => new(_repo.Object);

    private static UpdateProfileRequestDto ValidDto(string email = "updated@example.com")
        => new("Updated", "User", "+380993333333", email);

    [Fact]
    public async Task Handle_CustomerNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Customer?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => CreateHandler().Handle(new UpdateProfileCommand(99, ValidDto()), default));
    }

    [Fact]
    public async Task Handle_EmailAlreadyTaken_ThrowsDomainValidationException()
    {
        var customer = DomainObjectBuilder.CreateCustomer(email: "old@example.com");
        var other = DomainObjectBuilder.CreateCustomer(email: "taken@example.com");

        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(customer);
        _repo.Setup(r => r.GetByEmailAsync(It.Is<Email>(e => e.Value == "taken@example.com"), default))
             .ReturnsAsync(other);

        await Assert.ThrowsAsync<DomainValidationException>(
            () => CreateHandler().Handle(
                new UpdateProfileCommand(1, ValidDto("taken@example.com")), default));
    }

    [Fact]
    public async Task Handle_PhoneAlreadyTaken_ThrowsDomainValidationException()
    {
        var customer = DomainObjectBuilder.CreateCustomer(phone: "+380991234567");
        var other = DomainObjectBuilder.CreateCustomer(phone: "+380990000000", email: "other@example.com");

        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(customer);
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<Email>(), default))
             .ReturnsAsync((Shop.Domain.Models.Customer?)null);
        _repo.Setup(r => r.GetByPhoneAsync(It.Is<PhoneNumber>(p => p.Value == "+380990000000"), default))
             .ReturnsAsync(other);

        var dto = new UpdateProfileRequestDto("Updated", "User", "+380990000000", "new@example.com");
        await Assert.ThrowsAsync<DomainValidationException>(
            () => CreateHandler().Handle(new UpdateProfileCommand(1, dto), default));
    }

    [Fact]
    public async Task Handle_ValidData_SavesChanges()
    {
        var customer = DomainObjectBuilder.CreateCustomer();

        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(customer);
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<Email>(), default))
             .ReturnsAsync((Shop.Domain.Models.Customer?)null);
        _repo.Setup(r => r.GetByPhoneAsync(It.IsAny<PhoneNumber>(), default))
             .ReturnsAsync((Shop.Domain.Models.Customer?)null);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        await CreateHandler().Handle(new UpdateProfileCommand(1, ValidDto()), default);

        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
        Assert.Equal("Updated", customer.FirstName);
    }
}