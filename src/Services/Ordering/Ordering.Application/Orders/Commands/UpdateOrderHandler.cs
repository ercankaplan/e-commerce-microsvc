

namespace Ordering.Application.Orders.Commands;

public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderID = OrderId.Of(command.Order.Id);
        var existingOrder = await dbContext.Orders.FindAsync([orderID], cancellationToken);
        if (existingOrder == null)
        {
            throw new OrderNotFoundException(orderID.Value);
        }

        UpdateOrderWithNewValues(existingOrder, command.Order);
        dbContext.Orders.Update(existingOrder);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateOrderResult(true);
    }

    private void UpdateOrderWithNewValues(Order existingOrder, OrderDto orderDto)
    {

        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.Email, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.Email, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);


        existingOrder.Update(
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment,
            orderDto.Status);
       
        foreach (var item in orderDto.Items)
        {
            existingOrder.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
        }
    }
}
