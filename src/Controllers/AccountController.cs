using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Shop.ViewModels;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public AccountController(IAccountService accountService, IOrderService orderService)
        {
            _accountService = accountService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Profile()
        {
            var userIdString = Request.Cookies["userId"];

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                Response.Cookies.Delete("userId");
                return RedirectToAction("Login", "Auth");
            }

            var user = await _accountService.GetCurrentUserAsync(userIdString);

            if (user == null)
            {
                Response.Cookies.Delete("userId");
                return RedirectToAction("Login", "Auth");
            }

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            var userOrders = await _orderService.GetUserOrdersAsync(userId);

            var viewModel = new UserProfileViewModel
            {
                Customer = user,
                Orders = userOrders
            };

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("userId");
            return RedirectToAction("Login", "Auth");
        }
    }
}