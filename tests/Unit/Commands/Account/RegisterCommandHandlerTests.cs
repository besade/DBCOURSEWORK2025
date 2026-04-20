using Moq;
using Shop.Application.Commands.Account.Register;
using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Account;

public class RegisterCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repo = new();
    private readonly Mock<ICustomerFactory> _factory = new();
    private readonly Mock<IJwtTokenGenerator> _jwt = new();

    private RegisterCommandHandler CreateHandler()
        => new(_repo.Object, _factory.Object, _jwt.Object);

    [Fact]
    public async Task Handle_ValidData_CreatesCustomerAndReturnsToken()
    {
        var customer = DomainObjectBuilder.CreateCustomer(email: "new@example.com");
        _factory.Setup(f => f.CreateAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<string>()))
            .ReturnsAsync(customer);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);
        _jwt.Setup(j => j.GenerateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns("token-abc");

        var dto = new CustomerRegisterRequestDto(
            "New", "User", "+380991111111", "new@example.com",
            new DateOnly(1998, 1, 1), "Password123");

        var result = await CreateHandler().Handle(new RegisterCommand(dto), default);

        Assert.Equal("token-abc", result.Token);
        _repo.Verify(r => r.AddAsync(customer, default), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_FactoryThrows_PropagatesException()
    {
        _factory.Setup(f => f.CreateAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<string>()))
            .ThrowsAsync(new DomainValidationException("Email вже використовується."));

        var dto = new CustomerRegisterRequestDto(
            "Dup", "User", "+380992222222", "dup@example.com",
            new DateOnly(1998, 1, 1), "Password123");

        await Assert.ThrowsAsync<DomainValidationException>(
            () => CreateHandler().Handle(new RegisterCommand(dto), default));
    }
}