using Shop.Domain.Exceptions;
using static Shop.Domain.Models.ValueObjects;

namespace Shop.Domain.Models;

public partial class Customer
{
    public int CustomerId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public bool IsAdmin { get; private set; }

    public virtual ICollection<Address> Addresses { get; private set; } = new List<Address>();
    public virtual Cart Cart { get; private set; } = null!;
    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();
    public virtual ICollection<Review> Reviews { get; private set; } = new List<Review>();

    protected Customer() { }

    public Customer(
        string firstName,
        string lastName,
        PhoneNumber phoneNumber,
        Email email,
        DateOnly dateOfBirth,
        byte[] passwordHash,
        byte[] passwordSalt,
        bool isAdmin = false)
    {
        UpdatePersonalInfo(firstName, lastName, phoneNumber, email);

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-12)))
            throw new DomainValidationException("Користувач має бути старше 12 років.");

        if (dateOfBirth < DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-100)))
            throw new DomainValidationException("Максимальний вік, який можна встановити - 100 років.");

        DateOfBirth = dateOfBirth;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        IsAdmin = isAdmin;
        Cart = new Cart(this);
    }

    public void UpdatePersonalInfo(string firstName, string lastName, PhoneNumber phoneNumber, Email email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainValidationException("Ім'я не має бути порожнім.");

        if (firstName.Length > 25)
            throw new DomainValidationException("Ім'я занадто довге (макс. 25 символів).");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainValidationException("Прізвище не має бути порожнім.");

        if (lastName.Length > 25)
            throw new DomainValidationException("Прізвище занадто довге (макс. 25 символів).");

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}
