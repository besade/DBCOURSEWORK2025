using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Account.Login;
using Shop.Application.Commands.Account.Register;
using Shop.Application.Commands.Account.UpdateProfile;
using Shop.Application.Queries.Account.GetCustomerProfile;
using Shop.Presentation.RequestDTOs;
using Shop.Presentation.ViewModels;
using System.Security.Claims;
namespace Shop.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public IActionResult Login()
        => User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(CustomerLoginRequestDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _mediator.Send(new LoginCommand(dto));
        AppendJwtCookie(result.Token);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
        => User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CustomerRegisterRequestDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _mediator.Send(new RegisterCommand(dto));
        AppendJwtCookie(result.Token);
        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpGet]
    public async Task<IActionResult> Profile()
    {
        var profile = await _mediator.Send(new GetCustomerProfileQuery(GetCurrentUserId()));
        return View(new ProfileViewModel { Profile = profile });
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            var profile = await _mediator.Send(new GetCustomerProfileQuery(GetCurrentUserId()));
            return View("Profile", new ProfileViewModel { Profile = profile, UpdateDto = dto });
        }
        await _mediator.Send(new UpdateProfileCommand(GetCurrentUserId(), dto));
        TempData["Success"] = "Профіль успішно оновлено.";
        return RedirectToAction("Profile");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt-token");
        return RedirectToAction("Login");
    }

    private void AppendJwtCookie(string token) =>
        Response.Cookies.Append("jwt-token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}