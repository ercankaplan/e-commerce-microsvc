namespace Ordering.Domain.Exceptions
{
    public class DomainException:Exception
    {
        public DomainException(string message) 
            : base($"Domain Excepiton: \"{message}\" throws from Domain Layer."){ }
    }
}
