namespace Shop.Application.DTOs
{
        public record AddressResponseDto(int Id, string Country, string City, string AddressLine, bool IsDefault);
        public record AuthResponseDto(string Token, string FirstName, string Email, bool IsAdmin);
        public record CartItemResponseDto(int ProductId, string ProductName, decimal Price, int Quantity, byte[] Picture, decimal TotalPrice);
        public record CartResponseDto(IEnumerable<CartItemResponseDto> Items, decimal TotalPriceAmount);
        public record CategoriesListResponseDto(IEnumerable<CategoryResponseDto> Categories);
        public record CategoryResponseDto(int CategoryId, string CategoryName);
        public record CategorySalesResponseDto(string CategoryName, int TotalQuantitySold, decimal TotalRevenue);
        public record CustomerAddressesResponseDto(IEnumerable<AddressResponseDto> Addresses);
        public record CustomerProfileResponseDto(string FullName, string PhoneNumber, string Email, DateOnly DateOfBirth);
        public record CustomerSpendingResponseDto(string Email, int OrdersCount, decimal TotalSpent);
        public record PagedProductsListResponseDto(IEnumerable<PagedProductsResponseDto> Products, int TotalCount, int PageNumber);
        public record ProductsListResponseDto(IEnumerable<ProductShortResponseDto> Products);
        public record ProductResponseDto(string ProductName, string ProductCountry, decimal Weight, decimal Price, byte[] Picture, string CategoryName);
        public record ProductShortResponseDto(int ProductId, string ProductName, decimal Price, byte[] Picture, string CategoryName);
        public record PagedProductsResponseDto(int Id, string Name, decimal Price, byte[] Picture);
        public record OrderItemResponseDto(int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal TotalPrice, byte[] Picture);
        public record OrderResponseDto(int OrderId, DateOnly OrderDate, string Status, string DeliveryType, string Country, string City, string AddressLine, string RecipientFullName, 
            decimal TotalAmount, IEnumerable<OrderItemResponseDto> Items);
        public record OrdersListResponseDto(IEnumerable<OrderResponseDto> Orders);
}
