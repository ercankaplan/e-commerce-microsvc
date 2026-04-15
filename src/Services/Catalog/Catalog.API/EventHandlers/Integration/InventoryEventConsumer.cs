using BuildingBlocks.Messaging.Events.Inventory;
using Catalog.API.Products.ReserveProduct;
using MassTransit;

namespace Catalog.API.EventHandlers.Integration
{

    public sealed class InventoryEventConsumer(ILogger<InventoryEventConsumer> _logger, ISender sender)
        : IConsumer<IntEventReserveInventory>
    {
        public async Task Consume(ConsumeContext<IntEventReserveInventory> context)
        {


            //IntEventProcessPayment context.Message.Payload;

            var message = context.Message;

            try
            {

                var processPaymentEvent = context.Message;

                CancellationToken cancellationToken = context.CancellationToken;

                var command = MapToReserveProductCommand(processPaymentEvent);

                var processReservingResult = await sender.Send(command, cancellationToken);



            }
            catch (Exception ex)
            {


                _logger.LogError(ex, "Error processing inbox message {OrderId}", message.OrderId);
            }

        }

        private ReserveProductCommand MapToReserveProductCommand(IntEventReserveInventory? reserveInventoryEvent)
        {
            return new ReserveProductCommand(reserveInventoryEvent.OrderId, reserveInventoryEvent.ProductId, reserveInventoryEvent.Quantity);

        }
    }
}
