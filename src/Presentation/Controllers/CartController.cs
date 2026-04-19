using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Cart.AddItem;
using Shop.Application.Commands.Cart.DecreaseCartItemQuantity;
using Shop.Application.Commands.Cart.IncreaseCartItemQuantity;
using Shop.Application.Commands.Cart.RemoveItemFromCart;
using Shop.Application.Commands.Cart.UpdateCartItemQuantity;
using Shop.Application.Queries.Cart.GetCartContent;
using System.Security.Claims;
namespace Shop.Presentation.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IMediator _mediator;
    public CartController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await _mediator.Send(new GetCartContentQuery(GetCurrentUserId()));
        return View(cart);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId)
    {
        await _mediator.Send(new AddItemToCartCommand(GetCurrentUserId(), productId));
        TempData["Success"] = "Товар додано до кошика.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await _mediator.Send(new RemoveItemFromCartCommand(GetCurrentUserId(), productId));
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Increase(int productId)
    {
        await _mediator.Send(new IncreaseCartItemQuantityCommand(GetCurrentUserId(), productId));
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Decrease(int productId)
    {
        await _mediator.Send(new DecreaseCartItemQuantityCommand(GetCurrentUserId(), productId));
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        await _mediator.Send(new UpdateCartItemQuantityCommand(GetCurrentUserId(), productId, quantity));
        return RedirectToAction("Index");
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}