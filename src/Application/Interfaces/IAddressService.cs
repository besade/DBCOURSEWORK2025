using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Interfaces
{
    public interface IAddressService
    {
        Task AddAddressAsync(int customerId, AddressRequestDto dto, CancellationToken ct);
        Task SetDefaultAddressAsync(int customerId, int addressId, CancellationToken ct);
        Task UnsetDefaultAddressAsync(int customerId, int addressId, CancellationToken ct);
        Task DeleteAddressAsync(int customerId, int addressId, CancellationToken ct);
        Task RestoreAddressAsync(int customerId, int addressId, CancellationToken ct);

    }
}
