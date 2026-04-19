using Shop.Application.DTOs;
using Shop.Presentation.RequestDTOs;

namespace Shop.Presentation.ViewModels
{
    public class HomeViewModel
    {
        public PagedProductsListResponseDto Products { get; set; } = null!;
        public CategoriesListResponseDto Categories { get; set; } = null!;
        public int? SelectedCategoryId { get; set; }
    }

    public class ProfileViewModel
    {
        public CustomerProfileResponseDto? Profile { get; set; }
        public UpdateProfileRequestDto? UpdateDto { get; set; }
    }

    public class ProductFormViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductCountry { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Picture { get; set; }
        public CategoriesListResponseDto? Categories { get; set; }
    }

    public class OrderCreateViewModel
    {
        public CustomerAddressesResponseDto? Addresses { get; set; }
        public OrderRequestDto? Dto { get; set; }
    }

    public class CategoryManageViewModel
    {
        public CategoriesListResponseDto Categories { get; set; } = null!;
    }

    public class AnalyticsViewModel
    {
        public IEnumerable<CategorySalesResponseDto> SalesByCategory { get; set; } = [];
        public IEnumerable<CustomerSpendingResponseDto> TopCustomers { get; set; } = [];
    }
}