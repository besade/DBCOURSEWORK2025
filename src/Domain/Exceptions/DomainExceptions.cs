namespace Shop.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }

    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message) { }
    }

    public class DomainNotFoundException : DomainException
    {
        public DomainNotFoundException(string entityName, object key)
            : base($"Сутність '{entityName}' з ключем ({key}) не знайдена.") { }
    }
}
