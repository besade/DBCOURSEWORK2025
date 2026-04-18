using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IRepositories;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Application.Commands.Account.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(ICustomerRepository customerRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand command, CancellationToken ct)
        {
            var email = new Email(command.Dto.Email);
            var user = await _customerRepository.GetByEmailAsync(email, ct);

            if (user == null || !_passwordHasher.VerifyPassword(command.Dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new DomainValidationException("Невірний email або пароль.");

            var token = _jwtTokenGenerator.GenerateToken(user.CustomerId, user.Email.ToString(), user.IsAdmin);

            return new AuthResponseDto(token, user.FirstName, user.Email.ToString(), user.IsAdmin);
        }
    }
}
