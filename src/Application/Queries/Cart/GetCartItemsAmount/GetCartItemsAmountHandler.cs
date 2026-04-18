using MediatR;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Cart.GetCartItemsAmount
{
    public class GetCartItemsAmountQueryHandler : IRequestHandler<GetCartItemsAmountQuery, int>
    {
        private readonly ICartReadRepository _cartReadRepository;

        public GetCartItemsAmountQueryHandler(ICartReadRepository cartReadRepository)
        {
            _cartReadRepository = cartReadRepository;
        }

        public async Task<int> Handle(GetCartItemsAmountQuery query, CancellationToken ct)
        {
            return await _cartReadRepository.GetCountAsync(query.CustomerId, ct);
        }
    }
}
