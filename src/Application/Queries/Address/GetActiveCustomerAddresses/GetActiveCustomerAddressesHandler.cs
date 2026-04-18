using MediatR;
using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Address.GetByCustomerId
{
    public class GetActiveCustomerAddressesQueryHandler : IRequestHandler<GetCustomerActiveAddressesQuery, CustomerAddressesResponseDto>
    {
        private readonly IAddressReadRepository _readRepository;

        public GetActiveCustomerAddressesQueryHandler(IAddressReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<CustomerAddressesResponseDto> Handle(GetCustomerActiveAddressesQuery query, CancellationToken ct)
        {
            var addresses = await _readRepository.GetActiveByCustomerIdAsync(query.CustomerId, ct);

            if (addresses == null)
            {
                throw new NotFoundException(nameof(Address), query.CustomerId);
            }

            return addresses;
        }
    }
}
