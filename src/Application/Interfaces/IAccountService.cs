using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAccountService
    {
        Task<CustomerProfileResponseDto> GetProfileInfoAsync(int customerId, CancellationToken ct);
        Task UpdateProfileAsync(int customerId, UpdateProfileRequestDto dto, CancellationToken ct);
    }
}