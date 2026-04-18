using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Address.SetDefaultAddress
{
    public class SetDefaultAddressHandler : IRequestHandler<SetDefaultAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;

        public SetDefaultAddressHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task Handle(SetDefaultAddressCommand command, CancellationToken ct)
        {
            var targetAddress = await _addressRepository.GetByIdAsync(command.AddressId, ct);

            if (targetAddress == null || targetAddress.CustomerId != command.CustomerId)
                throw new NotFoundException(nameof(Address), command.AddressId);

            var addresses = await _addressRepository.GetByCustomerIdAsync(command.CustomerId, ct);

            foreach (var addr in addresses.Where(a => a.IsDefault))
            {
                addr.UnsetDefault();
            }

            targetAddress.SetAsDefault();

            await _addressRepository.SaveChangesAsync(ct);
        }
    }
}
