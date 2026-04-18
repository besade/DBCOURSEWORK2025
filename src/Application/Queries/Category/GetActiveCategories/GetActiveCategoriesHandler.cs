using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Category.GetActiveCategories
{
    public class GetActiveCategoriesQueryHandler : IRequestHandler<GetActiveCategoriesQuery, CategoriesListResponseDto>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetActiveCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<CategoriesListResponseDto> Handle(GetActiveCategoriesQuery query, CancellationToken ct)
        {
            return await _categoryReadRepository.GetAllActiveCategoriesAsync(ct);
        }
    }
}
