using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Application.DTOs;

namespace Shop.Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int customerId, OrderRequestDto dto, CancellationToken ct);
        Task<List<OrderResponseDto>> GetUserOrdersAsync(int customerId, CancellationToken ct);
        Task<List<OrderResponseDto>> GetAllOrdersAsync(CancellationToken ct);
        Task UpdateOrderStatusAsync(int orderId, Status newStatus, CancellationToken ct);
    }
}