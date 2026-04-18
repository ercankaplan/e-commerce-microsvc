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

        public static class OrderCreated
        {
            public const string Name = "order.created";
            public const int Version = 1;
        }

        public static class ProcessPayment
        {
            public const string Name = "payment.process";
            public const int Version = 1;
        }
    }
}
