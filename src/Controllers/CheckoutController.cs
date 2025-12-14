using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        public CheckoutController(IOrderService orderService, IAccountService accountService)
        {
            _orderService = orderService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Auth");

            var user = await _accountService.GetCurrentUserAsync(userIdString);

            var model = new CheckoutViewModel
            {
                RecipientFirstName = user?.FirstName ?? "",
                RecipientLastName = user?.LastName ?? "",
                CustomerIsRecipient = true
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var userIdString = Request.Cookies["userId"];

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(userId, model);

                return RedirectToAction("Success");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}