

using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orderList = await dbContext.Orders
                  .Include(o => o.ShippingAddress)
                  .Include(o => o.BillingAddress)
                  .Include(o => o.Payment)
                  .Include(o => o.OrderItems)
                  .AsNoTracking()
                  .Where(o => o.OrderName.Value.Contains(query.Name))
                  //.Select(o => new OrderDto(
                  //    o.Id.Value,
                  //    o.CustomerId.Value,
                  //    o.OrderName.Value,
                  //    new AddressDto(o.ShippingAddress.FirstName, o.ShippingAddress.LastName, o.ShippingAddress.Email, o.ShippingAddress.AddressLine, o.ShippingAddress.Country, o.ShippingAddress.State, o.ShippingAddress.ZipCode),
                  //    new AddressDto(o.BillingAddress.FirstName, o.BillingAddress.LastName, o.BillingAddress.Email, o.BillingAddress.AddressLine, o.BillingAddress.Country, o.BillingAddress.State, o.BillingAddress.ZipCode),
                  //    new PaymentDto(o.Payment.CardName, o.Payment.CardNumber, o.Payment.Expiration, o.Payment.CVV, o.Payment.PaymentMethod),
                  //    o.Status,
                  //    o.OrderItems.Select(i => new OrderItemDto(i.OrderId.Value, i.ProductId.Value, i.Quantity, i.Price)).ToList()))
                  .OrderBy(o => o.OrderName)
                  .ToListAsync(cancellationToken);

            var orderDtos = orderList.ToOrderDtos();

            return new GetOrdersByNameResult(orderDtos.AsReadOnly());
        }

        //private List<OrderDto> ProjectToOrderDto(List<Order> orders)
        //{
        //    return orders.Select(o => new OrderDto(
        //            o.Id.Value,
        //            o.CustomerId.Value,
        //            o.OrderName.Value,
        //            new AddressDto(o.ShippingAddress.FirstName, o.ShippingAddress.LastName, o.ShippingAddress.Email, o.ShippingAddress.AddressLine, o.ShippingAddress.Country, o.ShippingAddress.State, o.ShippingAddress.ZipCode),
        //            new AddressDto(o.BillingAddress.FirstName, o.BillingAddress.LastName, o.BillingAddress.Email, o.BillingAddress.AddressLine, o.BillingAddress.Country, o.BillingAddress.State, o.BillingAddress.ZipCode),
        //            new PaymentDto(o.Payment.CardName, o.Payment.CardNumber, o.Payment.Expiration, o.Payment.CVV, o.Payment.PaymentMethod),
        //            o.Status,
        //            o.OrderItems.Select(i => new OrderItemDto(i.OrderId.Value, i.ProductId.Value, i.Quantity, i.Price)).ToList()))
        //        .ToList();
        //}
    }
}
