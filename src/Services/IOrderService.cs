using Shop.Models;
using Shop.ViewModels;

namespace Shop.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int customerId, CheckoutViewModel model);
        Task<List<Order>> GetUserOrdersAsync(int customerId);
        Task<List<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, Status newStatus);
    }
}