using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events.Inventory
{
    public record IntEventReserveInventory
    {
        public Guid OrderId { get; init; }
    }
  
}
