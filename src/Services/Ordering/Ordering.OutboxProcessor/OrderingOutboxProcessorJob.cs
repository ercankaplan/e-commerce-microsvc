using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Data;

namespace Ordering.OutboxProcessor
{
    public class OrderingOutboxProcessorJob : BackgroundService
    {
        private const int BatchSize = 20;
        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(60);

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrderingOutboxProcessorJob> _logger;

        public OrderingOutboxProcessorJob(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<OrderingOutboxProcessorJob> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox processor started.");

            using var timer = new PeriodicTimer(PollInterval);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await ProcessOutboxMessages(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while processing outbox messages.");
                }
            }

            _logger.LogInformation("Outbox processor stopped.");
        }

        private async Task ProcessOutboxMessages(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var messages = await dbContext.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
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
                    var eventType = Type.GetType(message.EventType);
                    if (eventType is null)
                    {
                        message.RetryCount++;
                        message.LastError = $"Cannot resolve event type: {message.EventType}";
                        continue;
                    }

                    var domainEvent = JsonSerializer.Deserialize(message.Payload, eventType);
                    if (domainEvent is null)
                    {
                        message.RetryCount++;
                        message.LastError = "Deserialization returned null.";
                        continue;
                    }

                    await mediator.Publish(domainEvent, cancellationToken);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                    message.LastError = null;

                    _logger.LogInformation(
                        "Processed outbox message {MessageId} ({EventType})",
                        message.Id,
                        message.EventType);
                }
                catch (Exception ex)
                {
                    message.RetryCount++;
                    message.LastError = ex.ToString();

                    _logger.LogError(
                        ex,
                        "Error processing outbox message {MessageId}",
                        message.Id);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
