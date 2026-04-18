using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Queries
{
    public class AddressReadRepository : IAddressReadRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerAddressesResponseDto> GetActiveByCustomerIdAsync(int customerId, CancellationToken ct)
        {
            var result = await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerAddressesResponseDto(
                    c.Addresses
                    .Where(ci => !ci.IsDeleted).
                    Select(ci => new AddressResponseDto(
                        ci.AddressId,
                        ci.Country,
                        ci.City,
                        ci.AddressLine,
                        ci.IsDefault
                    )).ToList()
                ))
                .FirstOrDefaultAsync(ct);
            return result ?? new CustomerAddressesResponseDto(new List<AddressResponseDto>());
        }

        public async Task<CustomerAddressesResponseDto> GetByCustomerIdAsync(int customerId, CancellationToken ct)
        {
            var result = await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerAddressesResponseDto(
                    c.Addresses.Select(ci => new AddressResponseDto(
                        ci.AddressId,
                        ci.Country,
                        ci.City,
                        ci.AddressLine,
                        ci.IsDefault
                    )).ToList()
                ))
                .FirstOrDefaultAsync(ct);
            return result ?? new CustomerAddressesResponseDto(new List<AddressResponseDto>());
        }
    }
}
