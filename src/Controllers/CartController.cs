using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using System;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetCurrentCustomerId()
    {
        var userIdString = Request.Cookies["userId"];

        if (int.TryParse(userIdString, out int userId))
            return userId;

        return 0;
    }

    public async Task<IActionResult> Index()
    {
        int userId = GetCurrentCustomerId();
        if (userId == 0) return RedirectToAction("Login", "Auth");

        var cart = await _cartService.GetCartAsync(userId);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int id, int quantity = 1, string? returnUrl = null)
    {
        int userId = GetCurrentCustomerId();

        if (userId == 0)
            return RedirectToAction("Login", "Auth", new { returnUrl });

        if (id > 0 && quantity > 0)
        {
            await _cartService.AddItemAsync(userId, id, quantity);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        int userId = GetCurrentCustomerId();

        if (productId > 0)
        {
            await _cartService.UpdateQuantityAsync(userId, productId, quantity);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        int userId = GetCurrentCustomerId();

        if (productId > 0)
        {
            await _cartService.RemoveItemAsync(userId, productId);
        }

        return RedirectToAction(nameof(Index));
    }
}