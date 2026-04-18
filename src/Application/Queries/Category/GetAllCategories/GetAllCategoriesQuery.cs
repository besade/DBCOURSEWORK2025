using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Category.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<CategoriesListResponseDto>;
}
