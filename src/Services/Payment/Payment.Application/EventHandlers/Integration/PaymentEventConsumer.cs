using BuildingBlocks.Messaging.Events.Payment;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.EventHandlers.Domain;


namespace Payment.Application.EventHandlers.Integration
{

    public sealed class PaymentEventConsumer(ILogger<PaymentEventConsumer> _logger, ISender sender)
        : IConsumer<IntEventProcessPayment>,IConsumer<IntEventRefundPayment>
    {
        public async Task Consume(ConsumeContext<IntEventProcessPayment> context)
        {


     

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

        public async Task Consume(ConsumeContext<IntEventRefundPayment> context)
        {
            var message = context.Message;

            try
            {

                var processRefundEvent = context.Message;

                CancellationToken cancellationToken = context.CancellationToken;

                var command = new ProcessRefundCommand(processRefundEvent.OrderId, processRefundEvent.UserId, processRefundEvent.OrderTotal, processRefundEvent.ParentPaymentId);

                var processRefundResult = await sender.Send(command, cancellationToken);



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
