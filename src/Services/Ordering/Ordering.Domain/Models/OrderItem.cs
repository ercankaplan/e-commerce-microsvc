using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class OrderItem:Entity<OrderItemId>
    {
        internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            Id = OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
        public OrderId OrderId { get; private set; } = default!;
        public ProductId ProductId { get; private set; } = default!;
        public int Quantity { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        //public Payment Payment { get; set; }
        //public CustomerId CustomerId { get; set; }

        public static OrderItem Create(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegative(price);

            return new OrderItem(orderId, productId, quantity, price);
        }

    }
}
