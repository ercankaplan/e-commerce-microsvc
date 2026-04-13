using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events.Order
{
    public record IntEventOrderSubmitted(Guid OrderId ,  decimal Total,  string Email);
}
