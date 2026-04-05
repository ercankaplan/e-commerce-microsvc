using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class OutboxMessage
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
        public DateTime? NextRetryAfter { get; set; } //  // Exponential backoff: don't retry immediately
        public string? LastError { get; set; }

        
    }

    public class OutboxMessageSettings
    {

        public static int EventVersion = 1;
        public static int EnvelopeVersion = 1;
    }
}
