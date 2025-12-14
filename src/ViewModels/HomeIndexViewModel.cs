using Shop.Models;

namespace Shop.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    }
}