using Shop.DTOs;
using Shop.Models;

namespace Shop.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<Customer?> LoginAsync(LoginDto dto);
    }
}