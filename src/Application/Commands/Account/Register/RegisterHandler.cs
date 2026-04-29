using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Interfaces;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Account.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerFactory _customerFactory;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly INotificationSender _notificationSender;

        public RegisterCommandHandler(ICustomerRepository customerRepository, ICustomerFactory customerFactory, 
            IJwtTokenGenerator jwtTokenGenerator, INotificationSender notificationSender)
        {
            _customerRepository = customerRepository;
            _customerFactory = customerFactory;
            _jwtTokenGenerator = jwtTokenGenerator;
            _notificationSender = notificationSender;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand command, CancellationToken ct)
        {
            var dto = command.Dto;

            var customer = await _customerFactory.CreateAsync(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.DateOfBirth, dto.Password);

            await _customerRepository.AddAsync(customer, ct);
            await _customerRepository.SaveChangesAsync(ct);

            await _notificationSender.SendWelcomeMessageAsync(customer.Email.ToString(), customer.FirstName);

            var token = _jwtTokenGenerator.GenerateToken(customer.CustomerId, customer.Email.ToString(), customer.IsAdmin);

            return new AuthResponseDto(token, customer.FirstName, customer.Email.ToString(), customer.IsAdmin);
        }
    }
}
