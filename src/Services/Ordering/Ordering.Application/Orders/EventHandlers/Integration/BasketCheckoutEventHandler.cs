using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Orders.Commands;
using System.Linq;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    //public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    //    : IConsumer<BasketCheckoutEvent>
    //{
    //    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    //    {
    //        logger.LogInformation("Integration event Handled: {IntegrationEvent}", context.Message.GetType().Name);

    //        var command = MapToCreateOrderCommand(context.Message);


    //        await sender.Send(command);
    //    }

    //    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    //    {

    //        var addressDto = new AddressDto(message.FirstName, message.LastName, message.Email, message.AddressLine, message.Country, message.State, message.ZipCode);
    //        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, int.Parse(message.PaymentMethod));

    //        var orderID = Guid.NewGuid();

    //        var orderDto = new OrderDto(
    //            Id: orderID,
    //            BillingAddress: addressDto,
    //            ShippingAddress: addressDto,
    //            CustomerId: message.CustomerId,
    //            OrderName: message.UserName,
    //            Payment: paymentDto,
    //            Status: Ordering.Domain.Enums.OrderStatus.Pending,
    //            Items: message.Items.Select(i => new OrderItemDto(orderID, new Guid("5334C996-8457-4CF0-815C-ED2B77C4FF61"), i.Quantity, i.Price)).ToList()
    //            );

    //        return new CreateOrderCommand(orderDto);
    //    }
    //}
}
