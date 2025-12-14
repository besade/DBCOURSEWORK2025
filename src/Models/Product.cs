using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductCountry { get; set; } = null!;

    public decimal Weight { get; set; }

    public int StockQuantity { get; set; }

    public decimal Price { get; set; }

    public bool isDeleted { get; set; }

    public byte[] Picture { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
