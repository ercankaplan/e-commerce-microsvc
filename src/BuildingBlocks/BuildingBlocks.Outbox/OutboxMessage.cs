using BuildingBlocks.Messaging.Events;

namespace BuildingBlocks.Outbox
{

                
    public record OutboxMessage
    {

        public Guid Id { get; init; }
        public DateTime OccurredOnUtc { get; init; }

        public required string EventName { get; init; }
        public required string EventType { get; init; }
        public int EventVersion { get; init; }

        // Outbox envelope schema version
        public int EnvelopeVersion { get; init; } = 1;

        public required string ContentType { get; init; } = "application/json";
        public required string Payload { get; init; }
        public string? Metadata { get; init; }

        public DateTime? ProcessedOnUtc { get; set; }
        public int RetryCount { get; set; }
        public string? LastError { get; set; }

        protected static OutboxMessage Create(IntegrationEvent integrationEvent, string payload, string? metadata = null)
        {
            return new OutboxMessage
            {
                Id = integrationEvent.Id,
                OccurredOnUtc = integrationEvent.OccurredOnUtc,
                EventName = integrationEvent.EventName,
                EventType = integrationEvent.EventType,
                EventVersion = integrationEvent.EventVersion,
                ContentType = "application/json",
                Payload = payload,
                Metadata = metadata
            };
        }
    }
}