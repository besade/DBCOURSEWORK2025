using Shop.Application.Interfaces;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Domain.Models;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    // Add item to cart
    public async Task AddItemAsync(int customerId, int productId, int quantity, CancellationToken ct)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

        if (cart == null)
        {
            throw new DomainNotFoundException(nameof(Cart), customerId);
        }

        cart.AddItem(productId);

        _cartRepository.Update(cart);
        await _cartRepository.SaveChangesAsync(ct);
    }

    // Remove item from cart
    public async Task RemoveItemAsync(int customerId, int productId, CancellationToken ct)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

        if (cart == null)
        {
            throw new DomainNotFoundException(nameof(Cart), customerId);
        }

        cart.RemoveItem(productId);

        _cartRepository.Update(cart);
        await _cartRepository.SaveChangesAsync(ct);
    }

    // Change item quantity
    public async Task UpdateQuantityAsync(int customerId, int productId, int quantity, CancellationToken ct)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

        if (cart == null)
        {
            throw new DomainNotFoundException(nameof(Cart), customerId);
        }

        cart.UpdateItemQuantity(productId, quantity);

        _cartRepository.Update(cart);
        await _cartRepository.SaveChangesAsync(ct);
    }

    // Increase item quantity by one
    public async Task IncreaseQuantityAsync(int customerId, int productId, CancellationToken ct)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

        if (cart == null)
        {
            throw new DomainNotFoundException(nameof(Cart), customerId);
        }

        cart.IncreaseItemQuantity(productId);

        _cartRepository.Update(cart);
        await _cartRepository.SaveChangesAsync(ct);
    }

    // Decrease item quantity by one
    public async Task DecreaseQuantityAsync(int customerId, int productId, CancellationToken ct)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, ct);

        if (cart == null)
        {
            throw new DomainNotFoundException(nameof(Cart), customerId);
        }

        cart.DecreaseItemQuantity(productId);

        _cartRepository.Update(cart);
        await _cartRepository.SaveChangesAsync(ct);
    }
}