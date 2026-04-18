using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Account.GetCustomerProfile
{
    public record GetCustomerProfileQuery(int CustomerId) : IRequest<CustomerProfileResponseDto>;
}
