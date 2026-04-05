using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public record OutboxDeadLetterQueue
    {
        public Guid Id { get; set; }
        public Guid OriginalMessageId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public DateTime MovedToDlqOnUtc { get; set; }
    }
}
