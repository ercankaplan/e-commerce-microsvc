using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Saga
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        // Business data
        public decimal OrderTotal { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
