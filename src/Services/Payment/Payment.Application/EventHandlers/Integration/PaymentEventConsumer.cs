using BuildingBlocks.Messaging.Events.Payment;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands;


namespace Payment.Application.EventHandlers.Integration
{

    public sealed class PaymentEventConsumer(ILogger<PaymentEventConsumer> _logger, ISender sender)
        : IConsumer<IntEventProcessPayment>
    {
        public async Task Consume(ConsumeContext<IntEventProcessPayment> context)
        {


            //IntEventProcessPayment context.Message.Payload;

            var message = context.Message;

            try
            {

                var processPaymentEvent = context.Message;

                CancellationToken cancellationToken = context.CancellationToken;

                var command = MapToPaymentTransactionCommand(processPaymentEvent);

                var processPaymentResult = await sender.Send(command, cancellationToken);



            }
            catch (Exception ex)
            {


                _logger.LogError(ex, "Error processing inbox message {OrderId}", message.OrderId);
            }

        }

        private ProcessPaymentCommand MapToPaymentTransactionCommand(IntEventProcessPayment? processPaymentEvent)
        {
            return new ProcessPaymentCommand(processPaymentEvent.OrderId, processPaymentEvent.UserId, processPaymentEvent.Amount);

        }
    }
}
