using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.IReadRepositories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;

namespace Shop.Application.Queries.Account.GetCustomerProfile
{
    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, CustomerProfileResponseDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomerProfileQueryHandler(ICustomerRepository customerRepository, ICustomerReadRepository customerReadRepository)
        {
            _customerRepository = customerRepository;
            _customerReadRepository = customerReadRepository;
        }

        public async Task<CustomerProfileResponseDto> Handle(GetCustomerProfileQuery query, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(query.CustomerId, ct);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), query.CustomerId);
            }

            return (await _customerReadRepository.GetProfileByIdAsync(query.CustomerId, ct))!;
        }
    }
}
