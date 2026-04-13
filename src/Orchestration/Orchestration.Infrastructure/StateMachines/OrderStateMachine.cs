using BuildingBlocks.Messaging.Events.Inventory;
using BuildingBlocks.Messaging.Events.Order;
using BuildingBlocks.Messaging.Events.Payment;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {

        public State ProcessingPayment { get; private set; }
        public State ReservingInventory { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        public Event<IntEventOrderSubmitted> OrderSubmitted { get; private set; }
        public Event<IntEventPaymentProcessed> PaymentProcessed { get; private set; }
        public Event<IntEventInventoryReserved> InventoryReserved { get; private set; }
        public Event<IntEventOrderFailed> OrderFailed { get; private set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => PaymentProcessed, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => InventoryReserved, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderFailed, x => x.CorrelateById(m => m.Message.OrderId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Saga.OrderTotal = context.Message.Total;
                        context.Saga.CustomerEmail = context.Message.Email;
                        context.Saga.OrderDate = DateTime.UtcNow;
                    })
                    .PublishAsync(context => context.Init<IntEventProcessPayment>(new
                    {
                        OrderId = context.Saga.CorrelationId,
                        Amount = context.Saga.OrderTotal
                    }))
                    .TransitionTo(ProcessingPayment)
            );

            During(ProcessingPayment,
                When(PaymentProcessed)
                    .PublishAsync(context => context.Init<IntEventReserveInventory>(new
                    {
                        OrderId = context.Saga.CorrelationId
                    }))
                    .TransitionTo(ReservingInventory),
                When(OrderFailed)
                    .TransitionTo(Failed)
                    .Finalize()
            );

            During(ReservingInventory,
                When(InventoryReserved)
                    .PublishAsync(context => context.Init<IntEventOrderConfirmed>(new
                    {
                        OrderId = context.Saga.CorrelationId
                    }))
                    .TransitionTo(Completed)
                    .Finalize(),
                When(OrderFailed)
                    .PublishAsync(context => context.Init<IntEventRefundPayment>(new
                    {
                        OrderId = context.Saga.CorrelationId,
                        Amount = context.Saga.OrderTotal
                    }))
                    .TransitionTo(Failed)
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }

    }
}
