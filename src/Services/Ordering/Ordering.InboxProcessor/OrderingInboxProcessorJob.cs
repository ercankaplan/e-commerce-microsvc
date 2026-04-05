using BuildingBlocks.Messaging.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Infrastructure.Data;
using System.Text.Json;

namespace Ordering.InboxProcessor
{
    public class OrderingInboxProcessorJob : BackgroundService
    {
        private const int BatchSize = 20;
        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(20);

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrderingInboxProcessorJob> _logger;

        public OrderingInboxProcessorJob(
            ILogger<OrderingInboxProcessorJob> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Inbox processor started.");

            using var timer = new PeriodicTimer(PollInterval);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await ProcessInboxMessages(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while processing inbox messages.");
                }
            }

            _logger.LogInformation("Inbox processor stopped.");
        }

        private async Task ProcessInboxMessages(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var messages = await dbContext.InboxMessages
                .Where(x => x.ProcessedOnUtc == null && (x.NextRetryAfter == null || x.NextRetryAfter <= DateTime.UtcNow))
                .OrderBy(x => x.ReceivedOnUtc)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);

            if (messages.Count == 0)
            {
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    if (message.EventName == EventContracts.BasketCheckout.Name &&
                        message.EventVersion == EventContracts.BasketCheckout.Version)
                    {
                        var basketCheckoutEvent = JsonSerializer.Deserialize<BasketCheckoutEvent>(message.Payload);

                        if (basketCheckoutEvent is null)
                        {
                            message.RetryCount++;
                            message.LastError = "Deserialization returned null.";
                            continue;
                        }

                        var command = MapToCreateOrderCommand(basketCheckoutEvent);
                        await mediator.Send(command, cancellationToken);

                        message.ProcessedOnUtc = DateTime.UtcNow;
                        message.LastError = null;
                        message.NextRetryAfter = null;

                        _logger.LogInformation(
                            "Processed inbox message {MessageId} ({EventName} v{EventVersion})",
                            message.Id,
                            message.EventName,
                            message.EventVersion);
                    }
                    else
                    {
                        message.RetryCount++;
                        message.LastError = $"Unsupported integration event: {message.EventName} v{message.EventVersion}";
                    }

                    if (message.RetryCount > 5 && message.ProcessedOnUtc is null)
                    {
                        dbContext.InboxDeadLetterQueues.Add(new InboxDeadLetterQueue
                        {
                            Id = Guid.NewGuid(),
                            OriginalMessageId = message.Id,
                            Type = message.EventType,
                            Content = message.Payload,
                            Error = message.LastError ?? "Unknown error",
                            MovedToDlqOnUtc = DateTime.UtcNow
                        });

                        dbContext.InboxMessages.Remove(message);
                    }
                }
                catch (Exception ex)
                {
                    message.RetryCount++;
                    message.NextRetryAfter = DateTime.UtcNow.AddSeconds(Math.Pow(2, message.RetryCount));
                    message.LastError = ex.ToString();

                    _logger.LogError(ex, "Error processing inbox message {MessageId}", message.Id);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            var addressDto = new AddressDto(
                message.FirstName,
                message.LastName,
                message.Email,
                message.AddressLine,
                message.Country,
                message.State,
                message.ZipCode);

            var paymentMethod = int.TryParse(message.PaymentMethod, out var parsedPaymentMethod) ? parsedPaymentMethod : 0;

            var paymentDto = new PaymentDto(
                message.CardName,
                message.CardNumber,
                message.Expiration,
                message.CVV,
                paymentMethod);

            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAddress: addressDto,
                BillingAddress: addressDto,
                Payment: paymentDto,
                Status: OrderStatus.Pending,
                Items: message.Items
                    .Select(i => new OrderItemDto(
                        OrderId: orderId,
                        ProductId: new Guid("5334C996-8457-4CF0-815C-ED2B77C4FF61"),
                        Quantity: i.Quantity,
                        Price: i.Price))
                    .ToList());

            return new CreateOrderCommand(orderDto);
        }
    }
}