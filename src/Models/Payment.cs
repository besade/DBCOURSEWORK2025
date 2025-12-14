using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public string TransactionId { get; set; } = null!;

    public Status PaymentStatus { get; set; }

    public PaymentType Type { get; set; }

    public virtual Order Order { get; set; } = null!;
}
