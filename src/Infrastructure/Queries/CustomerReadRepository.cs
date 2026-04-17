using Microsoft.EntityFrameworkCore;
using Shop.Application.IQueries;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;

namespace Shop.Infrastructure.Queries
{
    public class CustomerReadRepository : ICustomerReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerProfileResponseDto?> GetProfileByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerProfileResponseDto(
                    $"{c.FirstName} {c.LastName}",
                    c.PhoneNumber,
                    c.Email,
                    c.DateOfBirth
                ))
                .FirstOrDefaultAsync(ct);
        }
    }
}
