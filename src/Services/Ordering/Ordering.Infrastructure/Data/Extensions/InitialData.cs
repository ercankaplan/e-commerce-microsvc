using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers => new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")), "John Doe","john.doe@example.com"),
            Customer.Create(CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")), "Jane Smith","jane.smith@example.com"),
            Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291ab")), "Alice Johnson","alice.johnson@example.com"),
            Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291ac")), "Bob Brown","bob.brown@example.com"),
        };

        private static IEnumerable<Address> Addresses => new List<Address>
        {
            Address.Of("John", "Doe","john.doe@example.com", "123 Main St, Anytown, USA", "USA", "CA", "12345"),
            Address.Of( "Jane", "Smith","jane.smith@example.com", "456 Elm St, Othertown, USA", "USA", "NY", "67890"),
            Address.Of( "Alice", "Johnson","alice.johnson@example.com", "789 Oak St, Sometown, USA", "USA", "TX", "11223"),
            Address.Of( "Bob", "Brown","bob.brown@example.com", "101 Pine St, Anycity, USA", "USA", "FL", "44556")
        };

        private static IEnumerable<Payment> Payments => new List<Payment>
        {
            Payment.Of("MasterCard","4111111111111111", "05/30", "123",0),
            Payment.Of("Visa","5500000000000004", "06/29", "456",0),
            Payment.Of("Amex","340000000000009", "07/28", "789",0),
            Payment.Of("Discover","6011000000000004", "08/27", "012",0)
        };

        public static IEnumerable<Product> Products => new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone X", 500.00m),
            Product.Create(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Samsung Galaxy S21", 400.00m),
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Google Pixel 6", 300.00m),
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "OnePlus 9", 350.00m)
        };

        public static IEnumerable<Order> OrdersWithItems(IEnumerable<Customer> customers, IEnumerable<Product> products)
        {
            // Use the actual customer and product entities that were seeded
            var customer1 = customers.First(c => c.Name == "John Doe");
            var customer2 = customers.First(c => c.Name == "Jane Smith");
            var customer3 = customers.First(c => c.Name == "Alice Johnson");

            var product1 = products.First(p => p.Name == "IPhone X");
            var product2 = products.First(p => p.Name == "Samsung Galaxy S21");
            var product3 = products.First(p => p.Name == "Google Pixel 6");
            var product4 = products.First(p => p.Name == "OnePlus 9");

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                customer1.Id,
                OrderName.Of("Order 1"),
                Addresses.Skip(0).First(),
                Addresses.Skip(0).First(),
                Payments.Skip(0).First()
            );
            order1.Add(product1.Id, 2, product1.Price);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                customer2.Id,
                OrderName.Of("Order 2"),
                Addresses.Skip(1).First(),
                Addresses.Skip(1).First(),
                Payments.Skip(1).First()
            );
            order2.Add(product2.Id, 3, product2.Price);

            var order3 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                customer3.Id,
                OrderName.Of("Order 3"),
                Addresses.Skip(2).First(),
                Addresses.Skip(2).First(),
                Payments.Skip(2).First()
            );
            order3.Add(product3.Id, 1, product3.Price);
            order3.Add(product4.Id, 1, product4.Price);

            return new List<Order> { order1, order2, order3 };
        }
    }
}