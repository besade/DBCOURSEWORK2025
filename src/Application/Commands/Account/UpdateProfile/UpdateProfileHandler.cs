using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Application.Commands.Account.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateProfileCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(UpdateProfileCommand command, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(command.CustomerId, ct);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), command.CustomerId);
            }

            var dto = command.Dto;

            var newEmail = new Email(dto.Email);
            var newPhoneNumber = new PhoneNumber(dto.PhoneNumber);

            if (customer.Email != newEmail &&
                await _customerRepository.GetByEmailAsync(newEmail, ct) != null)
            {
                throw new DomainValidationException("Вказаний Email вже використовується.");
            }

            if (customer.PhoneNumber != newPhoneNumber &&
                await _customerRepository.GetByPhoneAsync(newPhoneNumber, ct) != null)
            {
                throw new DomainValidationException("Вказаний номер телефону вже використовується.");
            }

            customer.UpdatePersonalInfo(dto.FirstName, dto.LastName, newPhoneNumber, newEmail);

            await _customerRepository.SaveChangesAsync(ct);
        }
    }
}
