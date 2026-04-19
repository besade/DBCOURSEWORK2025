using MediatR;

namespace Shop.Application.Commands.Address.DeleteAddress
{
    public record DeleteAddressCommand(int CustomerId, int AddressId) : IRequest;
}
