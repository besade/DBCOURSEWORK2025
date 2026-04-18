using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Category
{
    public int CategoryId { get; private set; }
    public string CategoryName { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    public virtual ICollection<Product> Products { get; private set; } = new List<Product>();

    protected Category() { }

    public Category(string categoryName)
    {
        UpdateName(categoryName);
        IsDeleted = false;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainValidationException("Назва категорії не може бути порожньою.");

        if (newName.Length > 30)
            throw new DomainValidationException("Назва категорії не може перевищувати 30 символів.");

        CategoryName = newName;
    }

    public void MarkAsDeleted()
    {
        if (IsDeleted == true)
        {
            throw new DomainValidationException("Вказана категорія вже видалена.");
        }

        IsDeleted = true;
    }

    public void Restore()
    {
        if (IsDeleted == false)
        {
            throw new DomainValidationException("Вказана категорія не видалена.");
        }

        IsDeleted = false;
    }
}
