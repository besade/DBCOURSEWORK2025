using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Address.SetDefaultAddress
{
    public record SetDefaultAddressCommand(int CustomerId, int AddressId) : IRequest;
}
