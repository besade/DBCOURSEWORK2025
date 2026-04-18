using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Address.RestoreAddress
{
    public class RestoreAddressHandler : IRequestHandler<RestoreAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;

        public RestoreAddressHandler(IAddressRepository addressRepository) => _addressRepository = addressRepository;

        public async Task Handle(RestoreAddressCommand command, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(command.AddressId, ct);

            if (address == null || address.CustomerId != command.CustomerId)
                throw new NotFoundException(nameof(Address), command.AddressId);

            address.Restore();

            await _addressRepository.SaveChangesAsync(ct);
        }
    }
}
