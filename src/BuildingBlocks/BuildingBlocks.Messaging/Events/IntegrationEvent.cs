namespace BuildingBlocks.Messaging.Events
{
    public record IntegrationEventXX
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

        // Logical event name (stable across namespaces/refactors if you want)
        public virtual string EventName => GetType().Name;

        // Event contract/payload version
        public virtual int EventVersion => 1;
        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
