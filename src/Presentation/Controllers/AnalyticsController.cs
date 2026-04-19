using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Queries.Analytics.GetSalesByCategory;
using Shop.Application.Queries.Analytics.GetTopSpendingCustomers;
using Shop.Presentation.ViewModels;
namespace Shop.Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class AnalyticsController : Controller
{
    private readonly IMediator _mediator;
    public AnalyticsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var sales = await _mediator.Send(new GetSalesByCategoryQuery());
        var customers = await _mediator.Send(new GetTopSpendingCustomersQuery());
        return View(new AnalyticsViewModel { SalesByCategory = sales, TopCustomers = customers });
    }
}