using Shop.Domain.Exceptions;

namespace Shop.Domain.Models;

public partial class Review
{
    public int ReviewId { get; private set; }
    public int CustomerId { get; private set; }
    public int ProductId { get; private set; }
    public string ReviewComment { get; private set; } = null!;
    public int Rating { get; private set; }
    public DateOnly ReviewDate { get; private set; }
    public bool IsDeleted { get; private set; }

    public virtual Customer Customer { get; private set; } = null!;
    public virtual Product Product { get; private set; } = null!;

    protected Review() { }

    public Review(int productId, int customerId, string text, int rating)
    {
        if (customerId <= 0)
            throw new DomainValidationException("CustomerID має бути більшим за 0.");

        if (productId <= 0)
            throw new DomainValidationException("ProductID має бути більшим за 0.");

        UpdateContent(text, rating);

        ProductId = productId;
        CustomerId = customerId;
        ReviewDate = DateOnly.FromDateTime(DateTime.UtcNow);
        IsDeleted = false;
    }

    public void UpdateContent(string comment, int rating)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new DomainValidationException("Текст відгуку не може бути порожнім.");

        if (comment.Length > 600)
            throw new DomainValidationException("Відгук занадто довгий (макс. 600 символів).");

        if (rating < 1 || rating > 5)
            throw new DomainValidationException("Оцінка має бути від 1 до 5 зірок.");

        ReviewComment = comment;
        Rating = rating;
    }

    public void MarkAsDeleted() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}
