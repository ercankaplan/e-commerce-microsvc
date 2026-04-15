using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events.Payment
{
    public record IntEventRefundPayment
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public decimal OrderTotal { get; init; }
        public Guid ParentPaymentId { get; init; }
    }
}
