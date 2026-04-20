using Moq;
using Shop.Application.Commands.Account.Login;
using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Tests.Unit.Commands.Account;

public class LoginCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repo = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwt = new();

    private LoginCommandHandler CreateHandler()
        => new(_repo.Object, _hasher.Object, _jwt.Object);

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthResponseDto()
    {
        var customer = DomainObjectBuilder.CreateCustomer(email: "user@example.com");
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<Email>(), default))
             .ReturnsAsync(customer);
        _hasher.Setup(h => h.VerifyPassword("ValidPass1", customer.PasswordHash, customer.PasswordSalt))
               .Returns(true);
        _jwt.Setup(j => j.GenerateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
               .Returns("jwt-token");

        var dto = new CustomerLoginRequestDto("user@example.com", "ValidPass1");
        var result = await CreateHandler().Handle(new LoginCommand(dto), default);

        Assert.Equal("jwt-token", result.Token);
        Assert.Equal(customer.FirstName, result.FirstName);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsDomainValidationException()
    {
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<Email>(), default))
             .ReturnsAsync((Shop.Domain.Models.Customer?)null);

        var dto = new CustomerLoginRequestDto("missing@example.com", "AnyPass1");
        await Assert.ThrowsAsync<DomainValidationException>(
            () => CreateHandler().Handle(new LoginCommand(dto), default));
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsDomainValidationException()
    {
        var customer = DomainObjectBuilder.CreateCustomer();
        _repo.Setup(r => r.GetByEmailAsync(It.IsAny<Email>(), default))
             .ReturnsAsync(customer);
        _hasher.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
               .Returns(false);

        var dto = new CustomerLoginRequestDto("test@example.com", "WrongPass1");
        await Assert.ThrowsAsync<DomainValidationException>(
            () => CreateHandler().Handle(new LoginCommand(dto), default));
    }
}