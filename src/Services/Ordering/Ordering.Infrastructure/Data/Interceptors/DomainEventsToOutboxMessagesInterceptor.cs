using System.Text.Json;
using System.Text.Json.Serialization;
using BuildingBlocks.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DomainEventsToOutboxMessages(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DomainEventsToOutboxMessages(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void DomainEventsToOutboxMessages(DbContext? dbContext)
        {
            if (dbContext is null)
            {
                return;
            }

            var aggregateRoots = dbContext.ChangeTracker
                .Entries<IAggregate>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            if (aggregateRoots.Count == 0)
            {
                return;
            }

            var domainEvents = aggregateRoots
                .SelectMany(aggregate =>
                {
                    var events = aggregate.DomainEvents.ToList();
                    aggregate.ClearDomainEvents();
                    return events;
                })
                .ToList();

            var outboxMessages = domainEvents
                .Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOnUtc = ToUtc(domainEvent.OccuredOn),
                    EventName = domainEvent.GetType().Name,
                    EventType = domainEvent.EventType,
                    EventVersion = OutboxMessageSettings.EventVersion,
                    EnvelopeVersion = OutboxMessageSettings.EnvelopeVersion,
                    ContentType = "application/json",
                    Payload = JsonSerializer.Serialize(domainEvent, SerializerOptions),
                    Metadata = null,
                })
                .ToList();

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        }

        private static DateTime ToUtc(DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
            };
        }
    }
}
