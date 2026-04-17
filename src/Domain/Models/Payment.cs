using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Payment
{
    public int PaymentId { get; private set; }
    public int OrderId { get; private set; }
    public DateOnly PaymentDate { get; private set; }
    public decimal Amount { get; private set; }
    public string TransactionId { get; private set; } = null!;

    public Status PaymentStatus { get; private set; }
    public PaymentType Type { get; private set; }
    public virtual Order Order { get; private set; } = null!;

    protected Payment() { }

    public Payment(int orderId, decimal amount, string transactionId, PaymentType type)
    {
        if (orderId <= 0)
            throw new DomainValidationException("OrderId має бути більшим за 0.");

        if (amount <= 0)
            throw new DomainValidationException("Сума оплати має бути більшою за 0.");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new DomainValidationException("TransactionId не може бути порожнім.");

        OrderId = orderId;
        Amount = amount;
        TransactionId = transactionId;
        Type = type;

        PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        PaymentStatus = Status.Pending;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (PaymentStatus == Status.Failed && (newStatus == Status.Success || newStatus == Status.Pending || newStatus == Status.Refunded))
            throw new DomainValidationException("Неможливо відновити відхилену транзакцію.");

        if (PaymentStatus == Status.Success && newStatus == Status.Pending)
            throw new DomainValidationException("Неможливо змінити статус успішної транзакції на \"В обробці\".");

        if (PaymentStatus == Status.Success && newStatus == Status.Failed)
            throw new DomainValidationException("Неможливо змінити статус успішної транзакції на \"Відхилено\".");

        if (PaymentStatus == Status.Refunded && (newStatus == Status.Failed || newStatus == Status.Pending || newStatus == Status.Success))
            throw new DomainValidationException("Неможливо відновити транзакцію, яку було повернено.");

        PaymentStatus = newStatus;
    }
}
