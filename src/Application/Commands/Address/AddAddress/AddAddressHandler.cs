using MediatR;
using Shop.Domain.Interfaces.IRepositories;
using AddressEntity = Shop.Domain.Models.Address;

namespace Shop.Application.Commands.Address.AddAddress
{
    public class AddAddressHandler : IRequestHandler<AddAddressCommand,int>
    {
        private readonly IAddressRepository _addressRepository;

        public AddAddressHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<int> Handle(AddAddressCommand command, CancellationToken ct)
        {
            if (command.Dto.IsDefault)
            {
                var addresses = await _addressRepository.GetByCustomerIdAsync(command.CustomerId, ct);

                foreach (var addr in addresses.Where(a => a.IsDefault))
                {
                    addr.UnsetDefault();
                }
            }

            var address = new AddressEntity(command.CustomerId, command.Dto.Country, command.Dto.City, command.Dto.AddressLine, command.Dto.IsDefault);

            await _addressRepository.AddAsync(address, ct);
            await _addressRepository.SaveChangesAsync(ct);

            return address.AddressId;
        }
    }
}
