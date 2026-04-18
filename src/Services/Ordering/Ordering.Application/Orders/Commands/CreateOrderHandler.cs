


using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands;


public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
   
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {

        var orderId = OrderId.Of(command.Order.Id);

        var existingOrder = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        if (existingOrder != null)
        {
            return new CreateOrderResult(existingOrder);
        }

        var newOrder = CreateNewOrder(command.Order);

        await dbContext.Orders.AddAsync(newOrder, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(newOrder);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName,orderDto.ShippingAddress.Email,orderDto.ShippingAddress.AddressLine,orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName,orderDto.BillingAddress.Email,orderDto.BillingAddress.AddressLine,orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
        
        var newOrder = Order.Create(
            OrderId.Of(orderDto.Id),
            CustomerId.Of(orderDto.CustomerId),
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment
        );

        foreach (var item in orderDto.Items)
        {
            newOrder.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
        }

        return newOrder;
    }
}

