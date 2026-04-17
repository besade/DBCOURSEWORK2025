using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class OrderItem
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public virtual Order Order { get; private set; } = null!;
    public virtual Product Product { get; private set; } = null!;

    protected OrderItem() { }

    public OrderItem(int productId, decimal unitPrice, int quantity)
    {
        if (productId <= 0)
            throw new DomainValidationException("ProductId має бути більшим за 0.");

        if (unitPrice <= 0)
            throw new DomainValidationException("Ціна за одиницю має бути більшою за 0.");

        if (quantity <= 0)
            throw new DomainValidationException("Кількість товару має бути не менше 1.");

        if (quantity > 99)
            throw new DomainValidationException("Ви не можете додати більше 99 одиниць одного товару в кошик.");

        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public decimal TotalPrice => UnitPrice * Quantity;
}
