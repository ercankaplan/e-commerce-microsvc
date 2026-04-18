using BuildingBlocks.Messaging.Events.Payment;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Domain.Events;

namespace Payment.Application.EventHandlers.Domain
{
    public class PaymentSucceededEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<PaymentSucceededEventHandler> logger)
        : INotificationHandler<PaymentSucceededDomainEvent>
    {
        public async Task Handle(PaymentSucceededDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handled:{DomainEvent} for OrderId:{OrderId}",
                notification.GetType().Name,
                notification.OrderId);

            var integrationEvent = new IntEventPaymentProcessed
            {
                OrderId = notification.OrderId,
                PaymentTransactionId = notification.PaymentId.Value
            };

            await publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
