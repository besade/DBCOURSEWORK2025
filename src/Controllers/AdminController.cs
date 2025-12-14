using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Shop.DTOs;

namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;

        public AdminController(IAccountService accountService, IAdminService adminService, IOrderService orderService   )
        {
            _accountService = accountService;
            _adminService = adminService;
            _orderService = orderService;
        }

        private async Task<bool> IsAdminUser()
        {
            var userId = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userId)) return false;

            var user = await _accountService.GetCurrentUserAsync(userId);
            return user?.IsAdmin == true;
        }

        // Admin Panel
        public async Task<IActionResult> Index()
        {
            if (!await IsAdminUser())
            {
                return Forbid();
            }
            return View();
        }

        // Products Dashboard
        public async Task<IActionResult> ManageProducts()
        {
            if (!await IsAdminUser()) return Forbid();

            var products = await _adminService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            if (!await IsAdminUser()) return Forbid();

            await PopulateCategoriesViewBag();

            return View(new ProductDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            if (!await IsAdminUser()) return Forbid();

            if (ModelState.IsValid)
            {
                await _adminService.CreateProductAsync(dto);
                return RedirectToAction(nameof(ManageProducts));
            }

            await PopulateCategoriesViewBag();
            return View(dto);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            var product = await _adminService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var dto = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                ProductCountry = product.ProductCountry,
                Weight = product.Weight,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            await PopulateCategoriesViewBag();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductDto dto)
        {
            if (!await IsAdminUser()) return Forbid();
            if (id != dto.ProductId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _adminService.UpdateProductAsync(id, dto);
                return RedirectToAction(nameof(ManageProducts));
            }

            await PopulateCategoriesViewBag();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            await _adminService.DeleteProductAsync(id);
            return RedirectToAction(nameof(ManageProducts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreProduct(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            await _adminService.RestoreProductAsync(id);
            return RedirectToAction(nameof(ManageProducts));
        }

        public async Task<IActionResult> ManageCategories()
        {
            if (!await IsAdminUser()) return Forbid();

            var categories = await _adminService.GetAllCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> CreateCategory()
        {
            if (!await IsAdminUser()) return Forbid();
            return View(new CategoryDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            if (!await IsAdminUser()) return Forbid();

            if (ModelState.IsValid)
            {
                await _adminService.CreateCategoryAsync(dto);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(dto);
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            var category = await _adminService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var dto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, CategoryDto dto)
        {
            if (!await IsAdminUser()) return Forbid();
            if (id != dto.CategoryId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _adminService.UpdateCategoryAsync(id, dto);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(dto);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            var category = await _adminService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            await _adminService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(ManageCategories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            if (!await IsAdminUser()) return Forbid();

            await _adminService.RestoreCategoryAsync(id);
            return RedirectToAction(nameof(ManageCategories));
        }

        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, Shop.Models.Status orderStatus)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, orderStatus);
            return RedirectToAction("ManageOrders");
        }

        private async Task PopulateCategoriesViewBag()
        {
            var categories = await _adminService.GetAllCategoriesAsync();
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                categories,
                "CategoryId",
                "CategoryName"
            );
        }

    }
}
