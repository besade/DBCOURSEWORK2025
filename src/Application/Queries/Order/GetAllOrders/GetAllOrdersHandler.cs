using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Order.GetUserOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, OrdersListResponseDto>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetAllOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<OrdersListResponseDto> Handle(GetAllOrdersQuery query, CancellationToken ct)
        {
            return await _orderReadRepository.GetAllAsync(ct);
        }
    }
}
