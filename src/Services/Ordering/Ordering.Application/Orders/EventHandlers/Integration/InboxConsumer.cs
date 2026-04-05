using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public sealed class InboxConsumer(IApplicationDbContext dbContext)
          : IConsumer<IntegrationEventMessage>
    {
        public async Task Consume(ConsumeContext<IntegrationEventMessage> context)
        {
            var inboxMessage = new InboxMessage
            {
                Id = context.Message.Id,
                ReceivedOnUtc = DateTime.UtcNow,
                EnvelopeVersion = context.Message.EnvelopeVersion,
                EventName = context.Message.EventName,
                EventType = context.Message.EventType,
                EventVersion = context.Message.EventVersion,
                ContentType = context.Message.ContentType,
                Payload = context.Message.Payload,
                Metadata = context.Message.Metadata
            };

            await dbContext.InboxMessages.AddAsync(inboxMessage, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
