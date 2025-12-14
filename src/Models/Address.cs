using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int CustomerId { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public bool AddressIsDefault { get; set; }

    public bool isDeleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
