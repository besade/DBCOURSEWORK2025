using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Commands.Product.CreateProduct;
using Shop.Application.Commands.Product.DeleteProduct;
using Shop.Application.Commands.Product.RestoreProduct;
using Shop.Application.Commands.Product.UpdateProduct;
using Shop.Application.Queries.Category.GetActiveCategories;
using Shop.Application.Queries.Product.GetAllProducts;
using Shop.Application.Queries.Product.GetProductInfo;
using Shop.Presentation.RequestDTOs;
using Shop.Presentation.ViewModels;
namespace Shop.Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _mediator.Send(new GetProductInfoQuery(id));
        return View(product);
    }

    [Authorize(Roles = "Admin"), HttpGet]
    public async Task<IActionResult> Manage()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return View(products);
    }

    [Authorize(Roles = "Admin"), HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _mediator.Send(new GetActiveCategoriesQuery());
        return View(new ProductFormViewModel { Categories = categories });
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid || vm.Picture == null)
        {
            vm.Categories = await _mediator.Send(new GetActiveCategoriesQuery());
            return View(vm);
        }
        using var ms = new MemoryStream();
        await vm.Picture.CopyToAsync(ms);
        var dto = new ProductRequestDto(vm.ProductName, vm.ProductCountry, vm.Weight,
            vm.Price, vm.StockQuantity, vm.CategoryId, ms.ToArray());
        await _mediator.Send(new CreateProductCommand(dto));
        TempData["Success"] = "Товар успішно створено.";
        return RedirectToAction("Manage");
    }

    [Authorize(Roles = "Admin"), HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _mediator.Send(new GetProductInfoQuery(id));
        var categories = await _mediator.Send(new GetActiveCategoriesQuery());
        ViewBag.ProductId = id;
        return View(new ProductFormViewModel
        {
            ProductName = product.ProductName,
            ProductCountry = product.ProductCountry,
            Weight = product.Weight,
            Price = product.Price,
            Categories = categories
        });
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel vm)
    {
        ModelState.Remove(nameof(vm.Picture));
        if (!ModelState.IsValid)
        {
            vm.Categories = await _mediator.Send(new GetActiveCategoriesQuery());
            ViewBag.ProductId = id;
            return View(vm);
        }
        byte[] pictureBytes = Array.Empty<byte>();
        if (vm.Picture is { Length: > 0 })
        {
            using var ms = new MemoryStream();
            await vm.Picture.CopyToAsync(ms);
            pictureBytes = ms.ToArray();
        }
        var dto = new ProductRequestDto(vm.ProductName, vm.ProductCountry, vm.Weight,
            vm.Price, vm.StockQuantity, vm.CategoryId, pictureBytes);
        await _mediator.Send(new UpdateProductCommand(id, dto));
        TempData["Success"] = "Товар успішно оновлено.";
        return RedirectToAction("Manage");
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        TempData["Success"] = "Товар видалено.";
        return RedirectToAction("Manage");
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _mediator.Send(new RestoreProductCommand(id));
        TempData["Success"] = "Товар відновлено.";
        return RedirectToAction("Manage");
    }
}