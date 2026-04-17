using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(CustomerRegisterRequestDto dto, CancellationToken ct);
        Task<AuthResponseDto> LoginAsync(CustomerLoginRequestDto dto, CancellationToken ct);
    }
}