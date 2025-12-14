using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICartService _cartService;

        public OrderService(ApplicationDbContext db, ICartService cartService)
        {
            _db = db;
            _cartService = cartService;
        }

        public async Task CreateOrderAsync(int customerId, CheckoutViewModel model)
        {
            var cartItems = await _cartService.GetCartContentsAsync();

            if (cartItems == null || !cartItems.Any()) return;

            var order = new Order
            {
                CustomerId = customerId,
                RecipientFirstName = model.RecipientFirstName,
                RecipientLastName = model.RecipientLastName,
                CustomerIsRecipient = model.CustomerIsRecipient,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderStatus = Status.Pending,
                Delivery = model.Delivery,
            };

            var productIds = cartItems.Keys.ToList();
            var products = await _db.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

            foreach (var product in products)
            {
                int qty = cartItems[product.ProductId];
                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = qty,
                    UnitPrice = product.Price
                };
                order.OrderItems.Add(orderItem);

                product.StockQuantity -= qty;
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var cart = await _db.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart != null)
            {
                _db.CartItems.RemoveRange(cart.CartItems);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<List<Order>> GetUserOrdersAsync(int customerId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, Status newStatus)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.OrderStatus = newStatus;
                await _db.SaveChangesAsync();
            }
        }
    }
}