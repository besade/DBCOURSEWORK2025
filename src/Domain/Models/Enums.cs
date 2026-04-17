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
    NovaPoshta,
    UkrPoshta,
    Meest
}

public enum PaymentType
{
    Card,
    Paypal,
    ApplePay,
    GooglePay
}