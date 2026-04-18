using MediatR;
using Shop.Application.Exceptions;
using Shop.Domain.Interfaces.IRepositories;

namespace Shop.Application.Commands.Address.DeleteAddress
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task Handle(DeleteAddressCommand command, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(command.AddressId, ct);

            if (address == null || address.CustomerId != command.CustomerId)
                throw new NotFoundException(nameof(Address), command.AddressId);

            address.MarkAsDeleted();

            await _addressRepository.SaveChangesAsync(ct);
        }
    }
}
