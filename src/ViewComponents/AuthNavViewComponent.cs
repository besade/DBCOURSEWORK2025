using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Shop.Models;

namespace Shop.ViewComponents
{
    public class AuthNavViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        public AuthNavViewComponent(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.Request.Cookies["userId"];
            Customer? user = null;

            if (!string.IsNullOrEmpty(userId))
            {
                user = await _accountService.GetCurrentUserAsync(userId);
            }

            return View(user);
        }
    }
}