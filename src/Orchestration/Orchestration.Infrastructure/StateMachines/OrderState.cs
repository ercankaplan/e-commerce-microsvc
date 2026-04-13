using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.StateMachines
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        // Business data
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal OrderTotal { get; set; }
        public string? PaymentRequestId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
