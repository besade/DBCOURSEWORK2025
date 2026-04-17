using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Domain.Factories
{
    public class CustomerFactory : ICustomerFactory
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CustomerFactory(ICustomerRepository customerRepository, IPasswordHasher passwordHasher)
        {
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Customer> CreateAsync(string firstName, string lastName, string email, string phone, DateOnly birthdayDate, string password)
        {
            var customerEmail = new Email(email);
            var customerPhone = new PhoneNumber(phone);

            if (await _customerRepository.GetByEmailAsync(customerEmail) != null)
                throw new DomainValidationException("Користувач з таким Email вже існує.");

            if (await _customerRepository.GetByPhoneAsync(customerPhone) != null)
                throw new DomainValidationException("Користувач з таким номером телефону вже існує.");

            if (password.Length < 8)
                throw new DomainValidationException("Мінімальна довжина паролю - 8 символів.");

            if (password.Length > 32)
                throw new DomainValidationException("Максимальна довжина паролю - 32 символи.");

            var (hash, salt) = _passwordHasher.HashPassword(password);

            return new Customer(firstName, lastName, customerPhone, customerEmail, birthdayDate, hash, salt);
        }
    }
}
