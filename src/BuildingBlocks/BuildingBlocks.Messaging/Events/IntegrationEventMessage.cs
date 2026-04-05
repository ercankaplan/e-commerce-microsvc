namespace BuildingBlocks.Messaging.Events
{
    public record IntegrationEventMessage
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

        // Logical event name (stable across namespaces/refactors if you want)
        public  string EventName { get; init; }

        // Event contract/payload version
        public  int EventVersion { get; init; }
        public string EventType { get; init; }

        // Outbox envelope schema version
        public int EnvelopeVersion { get; init; } = 1;

        public required string ContentType { get; init; } = "application/json";
        public required string Payload { get; init; }
        public string? Metadata { get; init; }
    }
}
