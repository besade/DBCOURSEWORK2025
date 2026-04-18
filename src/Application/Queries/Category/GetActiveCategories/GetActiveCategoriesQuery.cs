using MediatR;
using Shop.Application.DTOs;

namespace Shop.Application.Queries.Category.GetActiveCategories
{
    public record GetActiveCategoriesQuery() : IRequest<CategoriesListResponseDto>;
}
