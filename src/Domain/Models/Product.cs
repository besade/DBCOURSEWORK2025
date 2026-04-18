using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Product
{
    public int ProductId { get; private set; }
    public int CategoryId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string ProductCountry { get; private set; } = null!;
    public decimal Weight { get; private set; }
    public int StockQuantity { get; private set; }
    public decimal Price { get; private set; }
    public bool IsDeleted { get; private set; }
    public byte[] Picture { get; private set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();
    public virtual Category Category { get; private set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public virtual ICollection<Review> Reviews { get; private set; } = new List<Review>();

    protected Product() { }

    public Product(
        string name,
        string country,
        decimal weight,
        decimal price,
        int stock,
        int categoryId,
        byte[] picture)
    {
        UpdateDetails(name, country, weight, categoryId);
        UpdatePrice(price);
        UpdateStock(stock);
        UpdatePicture(picture);

        IsDeleted = false;
    }

    public void UpdateDetails(string name, string country, decimal weight, int categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Назва товару не може бути порожньою.");

        if (name.Length > 200)
            throw new DomainValidationException("Назва товару занадто довга (макс. 200 символів).");

        if (string.IsNullOrWhiteSpace(country))
            throw new DomainValidationException("Країна не може бути порожньою.");

        if (country.Length > 70)
            throw new DomainValidationException("Назва країни занадто довга (макс. 70 символів).");

        if (weight <= 0 || weight >= 100)
            throw new DomainValidationException("Вага має бути більшою за 0 та меншою за 100 кг (точність 5,3).");

        if (categoryId <= 0)
            throw new DomainValidationException("CategoryID має бути більшим за 0.");

        ProductName = name;
        ProductCountry = country;
        Weight = weight;
        CategoryId = categoryId;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0 || newPrice >= 1000000)
            throw new DomainValidationException("Ціна має бути від 0.01 до 999,999.99.");

        Price = newPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new DomainValidationException("Кількість товару в наявності не може бути від'ємною.");

        StockQuantity = quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainValidationException("Кількість для зменшення має бути більшою за 0.");

        if (StockQuantity < quantity)
            throw new DomainValidationException($"Недостатньо товару на складі. Доступно: {StockQuantity}");

        StockQuantity -= quantity;
    }

    public void UpdatePicture(byte[] picture)
    {
        if (picture == null || picture.Length == 0)
            throw new DomainValidationException("Зображення товару не може бути порожнім.");
        Picture = picture;
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted == true)
        {
            throw new DomainValidationException("Вказаний товар вже видалений.");
        }

        IsDeleted = true;
    }

    public void Restore()
    {
        if (IsDeleted == false)
        {
            throw new DomainValidationException("Вказаний товар не видалений.");
        }

        IsDeleted = false;
    }
}
