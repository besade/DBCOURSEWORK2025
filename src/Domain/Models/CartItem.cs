using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class CartItem
{
    public int CartId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }

    public virtual Cart Cart { get; private set; } = null!;
    public virtual Product Product { get; private set; } = null!;

    protected CartItem() { }

    public CartItem(int productId)
    {
        if (productId <= 0)
            throw new DomainValidationException("ProductId має бути більшим за 0.");

        ProductId = productId;
        Quantity = 1;
    }

    public void IncreaseQuantity()
    {
        if (Quantity >= 99)
            throw new DomainValidationException("Ви не можете додати більше 99 одиниць одного товару в кошик.");

        Quantity++;
    }

    public void DecreaseQuantity()
    {
        if (Quantity <= 1)
            throw new DomainValidationException("Кількість одиниць товару не може бути меншою за 1.");

        Quantity--;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainValidationException("Кількість товару має бути не менше 1.");

        if (newQuantity > 99)
            throw new DomainValidationException("Ви не можете додати більше 99 одиниць одного товару в кошик.");

        Quantity = newQuantity;
    }
}
