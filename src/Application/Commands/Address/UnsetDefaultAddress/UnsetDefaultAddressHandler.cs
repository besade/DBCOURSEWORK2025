using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Address.UnsetDefaultAddress
{
    public class UnsetDefaultAddressHandler : IRequestHandler<UnsetDefaultAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;

        public UnsetDefaultAddressHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task Handle(UnsetDefaultAddressCommand command, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(command.AddressId, ct);

            if (address == null || address.CustomerId != command.CustomerId)
                throw new NotFoundException(nameof(Address), command.AddressId);

            address.UnsetDefault();

            await _addressRepository.SaveChangesAsync(ct);
        }
    }
}
