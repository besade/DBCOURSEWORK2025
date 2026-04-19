using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Order.CreateOrder;
using Shop.Application.Commands.Order.UpdateOrderStatus;
using Shop.Application.Queries.Address.GetByCustomerId;
using Shop.Application.Queries.Order.GetUserOrders;
using Shop.Domain.Models;
using Shop.Presentation.RequestDTOs;
using Shop.Presentation.ViewModels;
using System.Security.Claims;
namespace Shop.Presentation.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orders = await _mediator.Send(new GetUserOrdersQuery(GetCurrentUserId()));
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var addresses = await _mediator.Send(new GetCustomerActiveAddressesQuery(GetCurrentUserId()));
        return View(new OrderCreateViewModel { Addresses = addresses });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            var addresses = await _mediator.Send(new GetCustomerActiveAddressesQuery(GetCurrentUserId()));
            return View(new OrderCreateViewModel { Addresses = addresses, Dto = dto });
        }
        var orderId = await _mediator.Send(new CreateOrderCommand(GetCurrentUserId(), dto));
        TempData["Success"] = $"Замовлення №{orderId} успішно оформлено!";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin"), HttpGet]
    public async Task<IActionResult> All()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return View(orders);
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, Status newStatus)
    {
        await _mediator.Send(new UpdateOrderStatusCommand(id, newStatus));
        TempData["Success"] = "Статус замовлення оновлено.";
        return RedirectToAction("All");
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}