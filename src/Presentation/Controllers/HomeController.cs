using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Queries.Category.GetActiveCategories;
using Shop.Application.Queries.Product.GetPagedProducts;
using Shop.Presentation.ViewModels;
namespace Shop.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    public HomeController(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> Index(int? categoryId, int page = 1)
    {
        var products = await _mediator.Send(new GetPagedProductsQuery(categoryId, page));
        var categories = await _mediator.Send(new GetActiveCategoriesQuery());
        return View(new HomeViewModel
        {
            Products = products,
            Categories = categories,
            SelectedCategoryId = categoryId
        });
    }

    public IActionResult Error(string message)
    {
        ViewData["ErrorMessage"] = message ?? "Виникла помилка під час обробки запиту.";

        return View();
    }
}