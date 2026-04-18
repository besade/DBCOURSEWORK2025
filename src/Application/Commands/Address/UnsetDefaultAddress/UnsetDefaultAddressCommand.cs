using MediatR;

namespace Shop.Application.Commands.Address.UnsetDefaultAddress
{
    public record UnsetDefaultAddressCommand(int CustomerId, int AddressId) : IRequest;
}
