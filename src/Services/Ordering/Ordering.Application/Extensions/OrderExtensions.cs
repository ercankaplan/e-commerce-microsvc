
namespace Ordering.Application
{
    public static class OrderExtensions
    {
        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDto(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.Email, order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode),
                new AddressDto(order.BillingAddress.FirstName, order.BillingAddress.LastName, order.BillingAddress.Email, order.BillingAddress.AddressLine, order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode),
                new PaymentDto(order.Payment.CardName, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.CVV, order.Payment.PaymentMethod),
                order.Status,
                order.OrderItems.Select(i => new OrderItemDto(i.OrderId.Value, i.ProductId.Value, i.Quantity, i.Price)).ToList());
        }

        public static List<OrderDto> ToOrderDtos(this List<Order> orders)
        {
            return orders.Select(o => o.ToOrderDto()).ToList();
        }
    }
}
