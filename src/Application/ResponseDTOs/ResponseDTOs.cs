namespace Shop.Application.DTOs
{
        public record AuthResponseDto(string Token, string FirstName, string Email, bool IsAdmin);
        public record CategorySalesResponseDto(string CategoryName, int TotalQuantitySold, decimal TotalRevenue);
        public record CartItemResponseDto(int ProductId, string ProductName, decimal Price, int Quantity, byte[] Picture, decimal TotalPrice);
        public record CartResponseDto(List<CartItemResponseDto> Items, decimal TotalPriceAmount);
        public record CategoryResponseDto(int CategoryId, string CategoryName);
        public record CustomerProfileResponseDto(string FullName, string PhoneNumber, string Email, DateOnly DateOfBirth);
        public record CustomerSpendingResponseDto(string Email, int OrdersCount, decimal TotalSpent);
        public record ProductResponseDto(string ProductName, string ProductCountry, decimal Weight, decimal Price, byte[] Picture, string CategoryName);
        public record ProductShortResponseDto(int ProductId, string ProductName, decimal Price, byte[] Picture, string CategoryName);
        public record PagedProductsResponseDto(int Id, string Name, decimal Price, byte[] Picture);
        public record OrderItemResponseDto(int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal TotalPrice, byte[] Picture);
        public record OrderResponseDto(int OrderId, DateOnly OrderDate, string Status, string DeliveryType, string Address, string RecipientFullName, 
            decimal TotalAmount, List<OrderItemResponseDto> Items);
}
