using Microsoft.EntityFrameworkCore;
using Shop.Application.IQueries;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;

namespace Shop.Infrastructure.Queries
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderResponseDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking() 
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderResponseDto(
                    o.OrderId,
                    o.OrderDate,
                    o.OrderStatus.ToString(),
                    o.Delivery.ToString(),
                    o.Address.AddressLine.ToString(),
                    $"{o.RecipientFirstName} {o.RecipientLastName}",
                    o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                    o.OrderItems.Select(oi => new OrderItemResponseDto(
                        oi.ProductId,
                        oi.Product.ProductName,
                        oi.UnitPrice,
                        oi.Quantity,
                        oi.UnitPrice * oi.Quantity,
                        oi.Product.Picture
                    )).ToList()
                ))
                .ToListAsync(ct);
        }

        public async Task<List<OrderResponseDto>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderResponseDto(
                    o.OrderId,
                    o.OrderDate,
                    o.OrderStatus.ToString(),
                    o.Delivery.ToString(),
                    o.Address.AddressLine.ToString(),
                    $"{o.Customer.FirstName} {o.Customer.LastName}",
                    o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                    o.OrderItems.Select(oi => new OrderItemResponseDto(
                        oi.ProductId,
                        oi.Product.ProductName,
                        oi.UnitPrice,
                        oi.Quantity,
                        oi.UnitPrice * oi.Quantity,
                        oi.Product.Picture
                    )).ToList()
                ))
                .ToListAsync(ct);
        }
    }
}
