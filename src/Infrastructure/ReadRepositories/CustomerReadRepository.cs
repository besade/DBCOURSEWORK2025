using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Infrastructure.Queries
{
    public class CustomerReadRepository : ICustomerReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerProfileResponseDto?> GetProfileByIdAsync(int customerId, CancellationToken ct = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
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
