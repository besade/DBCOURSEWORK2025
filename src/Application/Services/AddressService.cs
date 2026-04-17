using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // Create address
        public async Task AddAddressAsync(int customerId, AddressRequestDto dto, CancellationToken ct)
        {
            if (dto.IsDefault)
            {
                await ResetDefaultAddressAsync(customerId, ct);
            }

            var address = new Address(customerId, dto.Country, dto.City, dto.AddressLine, dto.IsDefault);

            await _addressRepository.AddAsync(address, ct);
            await _addressRepository.SaveChangesAsync(ct);
        }

        // Set address as default
        public async Task SetDefaultAddressAsync(int customerId, int addressId, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(addressId, ct);

            if (address == null || address.CustomerId != customerId)
                throw new DomainNotFoundException(nameof(Address), addressId);

            await ResetDefaultAddressAsync(customerId, ct);
            address.SetAsDefault();
            _addressRepository.Update(address);

            await _addressRepository.SaveChangesAsync(ct);
        }

        // Unset default address
        public async Task UnsetDefaultAddressAsync(int customerId, int addressId, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(addressId, ct);

            if (address == null || address.CustomerId != customerId)
                throw new DomainNotFoundException(nameof(Address), addressId);

            address.UnsetDefault();
            _addressRepository.Update(address);

            await _addressRepository.SaveChangesAsync(ct);
        }

        // Set all addresses as not default
        private async Task ResetDefaultAddressAsync(int customerId, CancellationToken ct)
        {
            var addresses = await _addressRepository.GetByCustomerIdAsync(customerId, ct);
            foreach (var addr in addresses.Where(a => a.AddressIsDefault))
            {
                addr.UnsetDefault();
            }
        }

        // Delete address
        public async Task DeleteAddressAsync(int customerId, int addressId, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(addressId, ct);

            if (address == null || address.CustomerId != customerId)
                throw new DomainNotFoundException(nameof(Address), addressId);

            address.MarkAsDeleted();
            _addressRepository.Update(address);

            await _addressRepository.SaveChangesAsync(ct);
        }

        // Restore address
        public async Task RestoreAddressAsync(int customerId, int addressId, CancellationToken ct)
        {
            var address = await _addressRepository.GetByIdAsync(addressId, ct);

            if (address == null || address.CustomerId != customerId)
                throw new DomainNotFoundException(nameof(Address), addressId);

            address.Restore();
            _addressRepository.Update(address);

            await _addressRepository.SaveChangesAsync(ct);
        }
    }
}
