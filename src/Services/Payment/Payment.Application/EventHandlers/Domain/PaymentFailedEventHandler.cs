using BuildingBlocks.Messaging.Events.Order;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Domain.Events;

namespace Payment.Application.EventHandlers.Domain
{
    public class PaymentFailedEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<PaymentFailedEventHandler> logger)
        : INotificationHandler<PaymentFailedDomainEvent>
    {
        public async Task Handle(PaymentFailedDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled:{DomainEvent} for OrderId:{OrderId}",
                notification.GetType().Name,
                notification.OrderId);

            var integrationEvent = new IntEventOrderFailed
            {
                OrderId = notification.OrderId,
                Reason = notification.Reason
            };

            await publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
