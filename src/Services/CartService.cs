using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddItemAsync(int customerId, int productId, int quantity)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = productId,
                Quantity = quantity
            };
            _db.CartItems.Add(cartItem);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<int> GetCartItemCountAsync(int customerId)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        return cart?.CartItems.Sum(i => i.Quantity) ?? 0;
    }

    public async Task<Cart?> GetCartAsync(int customerId)
    {
        return await _db.Carts
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task RemoveItemAsync(int customerId, int productId)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                _db.CartItems.Remove(cartItem);
                await _db.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateQuantityAsync(int customerId, int productId, int quantity)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    _db.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                await _db.SaveChangesAsync();
            }
        }
    }

    public async Task<Dictionary<int, int>> GetCartContentsAsync(int customerId)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
            return new Dictionary<int, int>();

        return cart.CartItems.ToDictionary(i => i.ProductId, i => i.Quantity);
    }
}