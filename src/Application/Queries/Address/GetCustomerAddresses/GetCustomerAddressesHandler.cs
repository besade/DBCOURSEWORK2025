using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Address.GetByCustomerId
{
    public class GetCustomerAddressesQueryHandler : IRequestHandler<GetCustomerAddressesQuery, CustomerAddressesResponseDto>
    {
        private readonly IAddressReadRepository _readRepository;

        public GetCustomerAddressesQueryHandler(IAddressReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<CustomerAddressesResponseDto> Handle(GetCustomerAddressesQuery query, CancellationToken ct)
        {
            var addresses = await _readRepository.GetByCustomerIdAsync(query.CustomerId, ct);

            if (addresses == null)
            {
                throw new NotFoundException(nameof(Address), query.CustomerId);
            }

            return addresses;
        }
    }
}
