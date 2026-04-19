using MediatR;

namespace Shop.Application.Commands.Address.SetDefaultAddress
{
    public record SetDefaultAddressCommand(int CustomerId, int AddressId) : IRequest;
}
