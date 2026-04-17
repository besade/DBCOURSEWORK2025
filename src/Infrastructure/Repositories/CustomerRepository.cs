using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, ct);
    }

    public async Task<Customer?> GetByEmailAsync(Email email, CancellationToken ct = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email, ct);
    }

    public async Task<Customer?> GetByPhoneAsync(PhoneNumber phone, CancellationToken ct = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phone, ct);
    }

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        await _context.Customers.AddAsync(customer, ct);
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}