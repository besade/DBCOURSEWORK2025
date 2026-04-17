using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerFactory _customerFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(ICustomerRepository customerRepository, ICustomerFactory customerFactory, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _customerRepository = customerRepository;
            _customerFactory = customerFactory;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // Register
        public async Task RegisterAsync(CustomerRegisterRequestDto dto, CancellationToken ct)
        {
            var customer = await _customerFactory.CreateAsync(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.DateOfBirth, dto.Password);

            await _customerRepository.AddAsync(customer, ct);
            await _customerRepository.SaveChangesAsync(ct);
        }

        // Login
        public async Task<AuthResponseDto> LoginAsync(CustomerLoginRequestDto dto, CancellationToken ct)
        {
            var email = new Email(dto.Email);

            var user = await _customerRepository.GetByEmailAsync(email, ct);

            if (user == null)
            {
                throw new DomainNotFoundException(nameof(Customer), dto.Email);
            }

            if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new DomainValidationException("Невірний пароль.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto(token, user.FirstName, user.Email.ToString(), user.IsAdmin);
        }
    }
}