using Shop.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Shop.Domain.Models
{
    public class ValueObjects
    {
        public record Email
        {
            public string Value { get; }

            public Email(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomainValidationException("Email не може бути порожнім.");

                if (value.Length > 60)
                    throw new DomainValidationException("Email занадто довгий.");

                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new DomainValidationException("Некоректний формат Email.");

                Value = value;
            }

            public static implicit operator string(Email email) => email.Value;
        }

        public record PhoneNumber
        {
            public string Value { get; }

            public PhoneNumber(string value)
            {
                if (!Regex.IsMatch(value, @"^\+380\d{9}$"))
                    throw new DomainValidationException("Номер телефону має бути у форматі +380XXXXXXXXX.");

                Value = value;
            }

            public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
        }
    }
}
