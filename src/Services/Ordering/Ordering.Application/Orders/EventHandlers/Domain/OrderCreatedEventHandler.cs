

using BuildingBlocks.Messaging.Events.Order;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) 
        : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domaintEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled:{DomainEvent}.", domaintEvent.GetType().Name);

            if(await featureManager.IsEnabledAsync("OrderFulfillment"))
            {

                var integrationEvent = new IntEventOrderSubmitted(domaintEvent.Order.Id.Value, domaintEvent.Order.TotalPrice, domaintEvent.Order.BillingAddress.Email);

                await publishEndpoint.Publish(integrationEvent, cancellationToken);
            }

        }
    }
}
