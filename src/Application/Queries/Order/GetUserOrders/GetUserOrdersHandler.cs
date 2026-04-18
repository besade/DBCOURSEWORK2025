using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Order.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, OrdersListResponseDto>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetUserOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<OrdersListResponseDto> Handle(GetUserOrdersQuery query, CancellationToken ct)
        {
            return await _orderReadRepository.GetByCustomerIdAsync(query.CustomerId, ct);
        }
    }
}
