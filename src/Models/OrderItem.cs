using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class OrderItem
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Order Orders { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
