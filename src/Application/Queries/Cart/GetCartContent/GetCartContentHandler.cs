using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Cart.GetCartContent
{
    public class GetCartContentQueryHandler : IRequestHandler<GetCartContentQuery, CartResponseDto>
    {
        private readonly ICartReadRepository _cartReadRepository;

        public GetCartContentQueryHandler(ICartReadRepository cartReadRepository)
        {
            _cartReadRepository = cartReadRepository;
        }

        public async Task<CartResponseDto> Handle(GetCartContentQuery query, CancellationToken ct)
        {
            return await _cartReadRepository.GetCartDetailsAsync(query.CustomerId, ct);
        }
    }
}
