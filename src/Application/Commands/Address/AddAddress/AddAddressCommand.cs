using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Address.AddAddress
{
    public record AddAddressCommand(int CustomerId, AddressRequestDto Dto) : IRequest<int>;
}
