using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.Application.Interfaces;

namespace Shop.Presentation.ViewComponents
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