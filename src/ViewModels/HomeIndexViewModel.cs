using Shop.Models;

namespace Shop.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}