using Shop.Application.Interfaces;
using Shop.Application.IQueries;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Application.DTOs;
using static Shop.Domain.Models.ValueObjects;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReadRepository _customerReadRepository;

        public AccountService(ICustomerRepository customerRepository, ICustomerReadRepository customerReadRepository)
        {
            _customerRepository = customerRepository;
            _customerReadRepository = customerReadRepository;
        }

        // Get User Profile Info
        public async Task<CustomerProfileResponseDto> GetProfileInfoAsync(int customerId, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, ct);

            if (customer == null)
            {
                throw new DomainNotFoundException(nameof(Customer), customerId);
            }

            return (await _customerReadRepository.GetProfileByIdAsync(customerId, ct))!;
        }

        // Update User Profile
        public async Task UpdateProfileAsync(int customerId, UpdateProfileRequestDto dto, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, ct);

            if (customer == null)
            {
                throw new DomainNotFoundException(nameof(Customer), customerId);
            }

            var newEmail = new Email(dto.Email);
            var newPhoneNumber = new PhoneNumber(dto.PhoneNumber);

            if (customer.Email != newEmail && await _customerRepository.GetByEmailAsync(newEmail, ct) != null)
            {
                throw new DomainValidationException("Вказаний Email вже використовується.");
            }

            if (customer.PhoneNumber != newPhoneNumber && await _customerRepository.GetByPhoneAsync(newPhoneNumber, ct) != null)
            {
                throw new DomainValidationException("Вказаний номер телефону вже використовується.");
            }

            customer.UpdatePersonalInfo(dto.FirstName, dto.LastName, newPhoneNumber, newEmail);

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync(ct);
        }
    }
}