using Shop.Models;

namespace Shop.Services
{
    public interface IAccountService
    {
        Task<Customer?> GetCurrentUserAsync(string userId);
    }
}