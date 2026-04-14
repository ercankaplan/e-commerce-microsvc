using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Events.Payment;
using MassTransit;
using Payment.Application.Commands;
using Payment.Application.Data;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Payment.Application.EventHandlers.Integration
{

    public sealed class PaymentEventConsumer(ISender sender, 
        IPublisher publisher,
        ILogger<PaymentEventConsumer> _logger, 
        CancellationToken cancellationToken)
        : IConsumer<IntegrationEventMessage>
    {
        public async Task Consume(ConsumeContext<IntegrationEventMessage> context)
        {


            //IntEventProcessPayment context.Message.Payload;

            var message = context.Message;

            try
            {
                if (message.EventName == EventContracts.ProcessPayment.Name &&
                    message.EventVersion == EventContracts.ProcessPayment.Version)
                {
                    var processPaymentEvent = JsonSerializer.Deserialize<IntEventProcessPayment>(message.Payload);



                    var command = MapToPaymentTransactionCommand(processPaymentEvent, message.Id);

                   var processPaymentResult = await sender.Send(command, cancellationToken);

                  


                }
                else
                {

                    _logger.LogError($"Unsupported integration event: {message.EventName} v{message.EventVersion}");
                }


            }
            catch (Exception ex)
            {


                _logger.LogError(ex, "Error processing inbox message {MessageId}", message.Id);
            }

        }

        private ProcessPaymentCommand MapToPaymentTransactionCommand(IntEventProcessPayment? processPaymentEvent, Guid ıd)
        {
            return new ProcessPaymentCommand(processPaymentEvent.OrderId, processPaymentEvent.UserId, processPaymentEvent.Amount);

        }
    }
}
