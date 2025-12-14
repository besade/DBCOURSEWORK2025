using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1, string? returnUrl = null)
        {
            if (id > 0 && quantity > 0)
            {
                try
                {
                    await _cartService.AddItemAsync(id, quantity);
                }
                catch (UnauthorizedAccessException)
                {
                    return RedirectToAction("Login", "Auth", new { returnUrl = returnUrl });
                }
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
            if (productId > 0)
            {
                await _cartService.UpdateQuantityAsync(productId, quantity);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            if (productId > 0)
            {
                await _cartService.RemoveItemAsync(productId);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}