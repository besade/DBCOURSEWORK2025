using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.AddressId == id, ct);
    }

    public async Task<IEnumerable<Address>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
    {
        return await _context.Addresses
            .Where(a => a.CustomerId == customerId)
            .Where(a => a.IsDeleted == false)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Address address, CancellationToken ct)
    {
        await _context.Addresses.AddAsync(address, ct);
    }

    public void Update(Address address)
    {
        _context.Addresses.Update(address);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}