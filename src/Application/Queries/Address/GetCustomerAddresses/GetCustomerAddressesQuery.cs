using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Address.GetByCustomerId
{
    public record GetCustomerAddressesQuery(int CustomerId) : IRequest<CustomerAddressesResponseDto>;
}
