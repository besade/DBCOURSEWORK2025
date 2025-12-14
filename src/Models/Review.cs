using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string ReviewComment { get; set; } = null!;

    public int Rating { get; set; }

    public DateOnly ReviewDate { get; set; }

    public bool isDeleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
