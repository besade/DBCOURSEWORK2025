using MediatR;
using Shop.Application.DTOs;
using Shop.Application.IReadRepositories;

namespace Shop.Application.Queries.Category.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, CategoriesListResponseDto>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetAllCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<CategoriesListResponseDto> Handle(GetAllCategoriesQuery query, CancellationToken ct)
        {
            return await _categoryReadRepository.GetAllCategoriesAsync(ct);
        }
    }
}
