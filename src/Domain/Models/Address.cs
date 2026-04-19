using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Address
{
    public int AddressId { get; private set; }
    public int CustomerId { get; private set; }
    public string Country { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string AddressLine { get; private set; } = null!;
    public bool IsDefault { get; private set; }
    public bool IsDeleted { get; private set; }

    public virtual Customer Customer { get; private set; } = null!;
    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();

    protected Address() { }

    public Address(int customerId, string country, string city, string addressLine, bool isDefault = false)
    {
        if (customerId <= 0)
            throw new DomainValidationException("CustomerId має бути більшим за 0.");

        UpdateDetails(country, city, addressLine);

        CustomerId = customerId;
        IsDefault = isDefault;
        IsDeleted = false;
    }

    public void UpdateDetails(string country, string city, string addressLine)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new DomainValidationException("Країна не може бути порожньою.");

        if (country.Length > 70)
            throw new DomainValidationException("Назва країни занадто довга (макс. 70 символів).");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainValidationException("Місто не може бути порожнім.");

        if (city.Length > 200)
            throw new DomainValidationException("Назва міста занадто довга (макс. 200 символів).");

        if (string.IsNullOrWhiteSpace(addressLine))
            throw new DomainValidationException("Адреса (вулиця, будинок) не може бути порожньою.");

        if (addressLine.Length > 400)
            throw new DomainValidationException("Назва адреси (вулиця, будинок) занадто довга (макс. 400 символів).");

        Country = country;
        City = city;
        AddressLine = addressLine;
    }

    public void SetAsDefault()
    {
        if (IsDefault == true)
        {
            throw new DomainValidationException("Вказана адреса вже вказана за замовчуванням.");
        }

        IsDefault = true;
    }
    public void UnsetDefault()
    {
        if (IsDefault == false)
        {
            throw new DomainValidationException("Вказана адреса не вказана за замовчуванням.");
        }

        IsDefault = false;
    }
    public void MarkAsDeleted()
    {
        if (IsDeleted == true)
        {
            throw new DomainValidationException("Вказана адреса вже видалена.");
        }

        IsDeleted = true;
    }

    public void Restore()
    {
        if (IsDeleted == false)
        {
            throw new DomainValidationException("Вказана адреса не видалена.");
        }

        IsDeleted = false;
    }
}
