using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events.Order
{
    public class IntEventOrderFailed
    {
        public Guid OrderId { get; init; }
        public string Reason { get; init; }
    }
}
