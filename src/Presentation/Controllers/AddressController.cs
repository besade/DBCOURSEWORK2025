using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Address.AddAddress;
using Shop.Application.Commands.Address.DeleteAddress;
using Shop.Application.Commands.Address.SetDefaultAddress;
using Shop.Application.Commands.Address.UnsetDefaultAddress;
using Shop.Application.Queries.Address.GetByCustomerId;
using Shop.Presentation.RequestDTOs;
using System.Security.Claims;
namespace Shop.Presentation.Controllers;

[Authorize]
public class AddressController : Controller
{
    private readonly IMediator _mediator;
    public AddressController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var addresses = await _mediator.Send(new GetCustomerActiveAddressesQuery(GetCurrentUserId()));
        return View(addresses);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddressRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте коректність введених даних адреси.";
            return RedirectToAction("Index");
        }
        await _mediator.Send(new AddAddressCommand(GetCurrentUserId(), dto));
        TempData["Success"] = "Адресу успішно додано.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SetDefault(int id)
    {
        await _mediator.Send(new SetDefaultAddressCommand(GetCurrentUserId(), id));
        TempData["Success"] = "Адресу встановлено за замовчуванням.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UnsetDefault(int id)
    {
        await _mediator.Send(new UnsetDefaultAddressCommand(GetCurrentUserId(), id));
        TempData["Success"] = "Статус адреси змінено.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteAddressCommand(GetCurrentUserId(), id));
        TempData["Success"] = "Адресу видалено.";
        return RedirectToAction("Index");
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}