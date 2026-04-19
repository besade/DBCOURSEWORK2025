using FluentValidation;
using Shop.Presentation.RequestDTOs;

namespace Shop.Presentation.Validation
{
    // Register
    public class CustomerRegisterValidator : AbstractValidator<CustomerRegisterRequestDto>
    {
        public CustomerRegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(60);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+380\d{9}$");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(32);
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Дата народження не може бути у майбутньому."); ;
        }
    }

    // Login
    public class CustomerLoginValidator : AbstractValidator<CustomerLoginRequestDto>
    {
        public CustomerLoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(60);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(32);
        }
    }

    // Profile update
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileRequestDto>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(25);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(60);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+380\d{9}$");
        }
    }

    // Address
    public class AddressValidator : AbstractValidator<AddressRequestDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Country).NotEmpty().MaximumLength(70);
            RuleFor(x => x.City).NotEmpty().MaximumLength(200);
            RuleFor(x => x.AddressLine).NotEmpty().MaximumLength(400);
        }
    }

    // Category
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().MaximumLength(30);
        }
    }

    // Product 
    public class ProductValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ProductCountry).NotEmpty().MaximumLength(70);
            RuleFor(x => x.Weight).GreaterThan(0).PrecisionScale(5, 3, false);
            RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(8, 2, false);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.Picture).NotEmpty();
        }
    }

    // Order
    public class OrderRequestValidator : AbstractValidator<OrderRequestDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.AddressId).GreaterThan(0);

            Unless(x => x.CustomerIsRecipient, () => {
                RuleFor(x => x.RecipientFirstName)
                    .NotEmpty().WithMessage("Вкажіть ім'я отримувача.")
                    .MaximumLength(25);
                RuleFor(x => x.RecipientLastName)
                    .NotEmpty().WithMessage("Вкажіть прізвище отримувача.")
                    .MaximumLength(25);
            });

            RuleFor(x => x.Delivery).NotEmpty();
            RuleFor(x => x.Delivery).IsInEnum().WithMessage("Виберіть службу доставки");
        }
    }
}
