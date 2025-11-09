using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class Customer:Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(CustomerId customerId, string name, string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var customer = new Customer
            {
                Id = customerId,
                Name = name,
                Email = email

            };

            return customer;
        }
    }
}
