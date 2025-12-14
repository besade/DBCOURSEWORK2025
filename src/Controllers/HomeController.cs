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

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService  )
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _productService.GetAllActiveCategoriesAsync();

            var allProducts = await _productService.GetAllActiveProductsAsync();

            var filteredProducts = allProducts;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                filteredProducts = allProducts.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            var viewModel = new HomeIndexViewModel
            {
                Products = filteredProducts,
                Categories = categories
            };

            ViewBag.CurrentCategoryId = categoryId;

            var cartContents = await _cartService.GetCartContentsAsync();
            ViewBag.CartContents = cartContents;

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
            {
                return NotFound();
            }

            return File(product.Picture, "image/jpeg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
