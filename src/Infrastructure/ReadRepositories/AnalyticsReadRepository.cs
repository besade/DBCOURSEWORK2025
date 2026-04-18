using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Infrastructure.Queries
{
    public class AnalyticsReadRepository : IAnalyticsReadRepository
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategorySalesResponseDto>> GetSalesByCategoryAsync(CancellationToken ct)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Order.OrderStatus == Status.Success)
                .GroupBy(oi => oi.Product.Category.CategoryName)
                .Select(g => new CategorySalesResponseDto(g.Key, g.Sum(x => x.Quantity), g.Sum(x => x.Quantity * x.UnitPrice)))
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync(ct);
        }

        public async Task<List<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderStatus == Status.Success)
                .GroupBy(o => o.Customer.Email)
                .Select(g => new CustomerSpendingResponseDto(g.Key, g.Count(), g.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice))))
                .Where(x => x.TotalSpent > 1000)
                .OrderByDescending(x => x.TotalSpent)
                .ToListAsync(ct);
        }
    }
}
