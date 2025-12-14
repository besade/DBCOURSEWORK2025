using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Customer?> GetCurrentUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            if (!int.TryParse(userId, out int id))
                return null;

            return await _db.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
        }
    }
}