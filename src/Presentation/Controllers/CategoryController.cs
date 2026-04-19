using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Category.CreateCategory;
using Shop.Application.Commands.Category.DeleteCategory;
using Shop.Application.Commands.Category.RestoreCategory;
using Shop.Application.Commands.Category.UpdateCategoryName;
using Shop.Application.Queries.Category.GetAllCategories;
using Shop.Presentation.RequestDTOs;
using Shop.Presentation.ViewModels;
namespace Shop.Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        return View(new CategoryManageViewModel { Categories = categories });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Введіть коректну назву категорії (до 30 символів).";
            return RedirectToAction("Index");
        }
        await _mediator.Send(new CreateCategoryCommand(dto));
        TempData["Success"] = "Категорію створено.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateName(int id, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            TempData["Error"] = "Назва категорії не може бути порожньою.";
            return RedirectToAction("Index");
        }
        await _mediator.Send(new UpdateCategoryNameCommand(id, newName));
        TempData["Success"] = "Назву категорії оновлено.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        TempData["Success"] = "Категорію видалено.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _mediator.Send(new RestoreCategoryCommand(id));
        TempData["Success"] = "Категорію відновлено.";
        return RedirectToAction("Index");
    }
}