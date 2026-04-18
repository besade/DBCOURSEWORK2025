using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Address.GetByCustomerId
{
    public record GetCustomerActiveAddressesQuery(int CustomerId) : IRequest<CustomerAddressesResponseDto>;
}
