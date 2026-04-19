namespace Shop.Domain.Models;

public enum Status
{
    Pending,          
    Success,
    Failed,
    Refunded
}

public enum DeliveryType
{
    NovaPoshta = 1,
    UkrPoshta = 2,
    Meest = 3
}

public enum PaymentType
{
    Card,
    Paypal,
    ApplePay,
    GooglePay
}