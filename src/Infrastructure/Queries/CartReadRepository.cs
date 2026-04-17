using Microsoft.EntityFrameworkCore;
using Shop.Application.IQueries;
using Shop.Infrastructure.Data;
using Shop.Application.DTOs;

namespace Shop.Infrastructure.Queries
{
    public class CartReadRepository : ICartReadRepository
    {
        private readonly ApplicationDbContext _context;

        public CartReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync(int customerId, CancellationToken ct = default)
        {
            return await _context.CartItems
                .AsNoTracking()
                .Where(ci => ci.Cart.CustomerId == customerId)
                .SumAsync(ci => ci.Quantity, ct);
        }

        public async Task<CartResponseDto?> GetCartDetailsAsync(int customerId, CancellationToken ct = default)
        {
            return await _context.Carts
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CartResponseDto(
                
                    c.CartItems.Select(ci => new CartItemResponseDto(
                        ci.ProductId,
                        ci.Product.ProductName,
                        ci.Product.Price,
                        ci.Quantity,
                        ci.Product.Picture,
                        ci.Quantity * ci.Product.Price
                    )).ToList(),
                    c.CartItems.Sum(ci => ci.Quantity * ci.Product.Price)
                ))
                .FirstOrDefaultAsync(ct);
        }
    }
}
