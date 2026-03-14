using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Services;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<HomeController> _logger;
        private readonly ICartService _cartService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        private int GetCurrentCustomerId()
        {
            var userIdString = Request.Cookies["userId"];

            if (int.TryParse(userIdString, out int userId))
                return userId;

            return 0;
        }

        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            int pageSize = 6;
            if (page < 1) page = 1;

            var categories = await _productService.GetAllActiveCategoriesAsync();

            var (products, totalCount) =
                await _productService.GetPagedProductsAsync(categoryId, page, pageSize);

            var viewModel = new HomeIndexViewModel
            {
                Products = products,
                Categories = categories,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                SelectedCategoryId = categoryId
            };

            ViewBag.CurrentCategoryId = categoryId;

            int userId = GetCurrentCustomerId();

            if (userId != 0)
                ViewBag.CartContents = await _cartService.GetCartContentsAsync(userId);
            else
                ViewBag.CartContents = new Dictionary<int, int>();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductImage(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null || product.Picture == null || product.Picture.Length == 0)
                return NotFound();

            return File(product.Picture, "image/jpeg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}