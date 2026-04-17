using Shop.Domain.Models;

namespace Shop.Presentation.RequestDTOs
{
        public record AddressRequestDto(string Country, string City, string AddressLine, bool IsDefault);
        public record CategoryRequestDto(string CategoryName);
        public record CustomerRegisterRequestDto(string FirstName, string LastName, string PhoneNumber, string Email, DateOnly DateOfBirth, string Password);
        public record CustomerLoginRequestDto(string Email, string Password);
        public record OrderRequestDto(int AddressId, DeliveryType Delivery, string RecipientFirstName, string RecipientLastName, bool CustomerIsRecipient);
        public record ProductRequestDto(string ProductName, string ProductCountry, decimal Weight, decimal Price, int StockQuantity, int CategoryId, byte[] Picture);
        public record UpdateProfileRequestDto(string FirstName, string LastName, string PhoneNumber, string Email);
}
