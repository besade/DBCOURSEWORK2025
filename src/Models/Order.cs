using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int? AddressId { get; set; }

    public string RecipientFirstName { get; set; } = null!;

    public string RecipientLastName { get; set; } = null!;

    public bool CustomerIsRecipient { get; set; }

    public DateOnly OrderDate { get; set; }

    public Status OrderStatus { get; set; }

    public DeliveryType Delivery { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
