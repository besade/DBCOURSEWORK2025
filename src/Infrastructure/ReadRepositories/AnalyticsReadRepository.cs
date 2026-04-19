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
            var rawData = await _context.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Order.OrderStatus == Status.Success)
                .GroupBy(oi => oi.Product.Category.CategoryName)
                .Select(g => new
                    {
                        CategoryName = g.Key,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                    })
                .OrderByDescending(x => x.Revenue)
                .ToListAsync(ct);

            return rawData.Select(x => new CategorySalesResponseDto(x.CategoryName, x.TotalQuantity, x.Revenue)).ToList();
        }

        public async Task<List<CustomerSpendingResponseDto>> GetTopSpendingCustomersAsync(CancellationToken ct)
        {
            var rawData = await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderStatus == Status.Success)
                .GroupBy(o => o.Customer.Email)
                .Select(g => new
                    {
                        Email = g.Key,
                        OrderCount = g.Count(),
                        Spent = g.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity * oi.UnitPrice)
                    })
                .Where(x => x.Spent > 1000)
                .OrderByDescending(x => x.Spent)
                .ToListAsync(ct);

            return rawData.Select(x => new CustomerSpendingResponseDto(x.Email, x.OrderCount, x.Spent)).ToList();
        }
    }
}
