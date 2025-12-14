using NpgsqlTypes;

namespace Shop.Models;

public enum Status
{
    [PgName("pending")] 
    Pending,          

    [PgName("success")]
    Success,

    [PgName("failed")]
    Failed,

    [PgName("refunded")]
    Refunded
}

public enum DeliveryType
{
    [PgName("nova_poshta")]
    NovaPoshta,

    [PgName("ukr_poshta")]
    UkrPoshta,

    [PgName("meest")]
    Meest
}

public enum PaymentType
{
    [PgName("card")]
    Card,

    [PgName("paypal")]
    Paypal,

    [PgName("apple_pay")]
    ApplePay,

    [PgName("google_pay")]
    GooglePay
}