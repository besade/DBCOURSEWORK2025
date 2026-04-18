using MediatR;

namespace Shop.Application.Commands.Address.RestoreAddress
{
    public record RestoreAddressCommand(int CustomerId, int AddressId) : IRequest;
}
