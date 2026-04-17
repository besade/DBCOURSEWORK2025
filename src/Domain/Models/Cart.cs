using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Cart
{
    public int CartId { get; private set; }
    public int CustomerId { get; private set; }

    public virtual ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();
    public virtual Customer Customer { get; private set; } = null!;

    protected Cart() { }

    internal Cart(Customer customer)
    {
        Customer = customer;
    }

    public void AddItem(int productId)
    {
        var existingItem = CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem != null)
        {
            throw new DomainValidationException("Даний товар вже існує у кошику.");
        }
        else
        {
            var newItem = new CartItem(productId);
            CartItems.Add(newItem);
        }
    }

    public void IncreaseItemQuantity(int productId)
    {
        var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            throw new DomainNotFoundException(nameof(CartItem), productId);
        }

        item.IncreaseQuantity();
    }

    public void DecreaseItemQuantity(int productId)
    {
        var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            throw new DomainNotFoundException(nameof(CartItem), productId);
        }

        item.DecreaseQuantity();
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            throw new DomainNotFoundException(nameof(CartItem), productId);
        }

        item.UpdateQuantity(quantity);
    }

    public void RemoveItem(int productId)
    {
        var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
        {
            throw new DomainNotFoundException(nameof(CartItem), productId);
        }

        CartItems.Remove(item);
    }

    public void Clear()
    {
        CartItems.Clear();
    }
}
