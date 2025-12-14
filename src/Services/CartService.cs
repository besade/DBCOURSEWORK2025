using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetCurrentCustomerId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.Request.Cookies["userId"];

        if (int.TryParse(userIdString, out int userId))
        {
            return userId;
        }

        return 0;
    }

    public async Task AddItemAsync(int productId, int quantity)
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0)
        {
            throw new UnauthorizedAccessException("User must be logged in to modify the cart.");
        }

        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = userId };
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

    public async Task<int> GetCartItemCountAsync()
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return 0;

        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);

        return cart?.CartItems.Sum(i => i.Quantity) ?? 0;
    }

    public async Task<Cart?> GetCartAsync()
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return null;

        return await _db.Carts
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);
    }
    public async Task RemoveItemAsync(int productId)
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return;

        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);

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

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return;

        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);

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


    public async Task<Dictionary<int, int>> GetCartContentsAsync()
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return new Dictionary<int, int>();

        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);

        if (cart == null)
        {
            return new Dictionary<int, int>();
        }

        return cart.CartItems.ToDictionary(i => i.ProductId, i => i.Quantity);
    }
}