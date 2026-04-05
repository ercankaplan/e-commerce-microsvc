using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public static class EventContracts
    {

        public static class BasketCheckout
        {
            public const string Name = "basket.checkout";
            public const int Version = 1;
        }
    }
}
