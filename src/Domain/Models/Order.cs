using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Order
{
    public int OrderId { get; private set; }
    public int CustomerId { get; private set; }
    public int AddressId { get; private set; }
    public string RecipientFirstName { get; private set; } = null!;
    public string RecipientLastName { get; private set; } = null!;
    public bool CustomerIsRecipient { get; private set; }
    public DateOnly OrderDate { get; private set; }

    public Status OrderStatus { get; private set; }
    public DeliveryType Delivery { get; private set; }
    public virtual Address Address { get; private set; } = null!;
    public virtual Customer Customer { get; private set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public virtual Payment Payment { get; private set; } = null!;

    protected Order() { }

    public Order(
        int customerId,
        int addressId,
        string recipientFirstName,
        string recipientLastName,
        DeliveryType delivery,
        bool customerIsRecipient = true)
    {
        if (customerId <= 0)
            throw new DomainValidationException("CustomerId має бути більшим за 0.");

        if (addressId <= 0)
            throw new DomainValidationException("AddressId має бути вказаний.");

        if (string.IsNullOrWhiteSpace(recipientFirstName))
            throw new DomainValidationException("Ім'я отримувача не має бути порожнім.");

        if (recipientFirstName.Length > 25)
            throw new DomainValidationException("Ім'я отримувача занадто довге (макс. 25 символів).");

        if (string.IsNullOrWhiteSpace(recipientLastName))
            throw new DomainValidationException("Прізвище отримувача не має бути порожнім.");

        if (recipientLastName.Length > 25)
            throw new DomainValidationException("Прізвище отримувача занадто довге (макс. 25 символів).");

        CustomerId = customerId;
        AddressId = addressId;
        Delivery = delivery;
        CustomerIsRecipient = customerIsRecipient;
        RecipientFirstName = recipientFirstName;
        RecipientLastName = recipientLastName;

        OrderDate = DateOnly.FromDateTime(DateTime.UtcNow);
        OrderStatus = Status.Pending;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (OrderStatus == Status.Failed && (newStatus == Status.Success || newStatus == Status.Pending || newStatus == Status.Refunded))
            throw new DomainValidationException("Неможливо відновити провалене замовлення.");

        if (OrderStatus == Status.Success && newStatus == Status.Pending)
            throw new DomainValidationException("Неможливо змінити статус успішного замовлення на \"В обробці\".");

        if (OrderStatus == Status.Success && newStatus == Status.Failed)
            throw new DomainValidationException("Неможливо змінити статус успішного замовлення на \"Відхилено\".");

        if (OrderStatus == Status.Refunded && (newStatus == Status.Failed || newStatus == Status.Pending || newStatus == Status.Success))
            throw new DomainValidationException("Неможливо відновити замовлення, яке було повернено.");

        OrderStatus = newStatus;
    }
}
