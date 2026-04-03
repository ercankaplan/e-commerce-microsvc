using Basket.Domain.Models;
using Marten;
using MassTransit.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Basket.OutboxProcessor
{
    public class BasketOutboxProcessorJob : BackgroundService
    {
 
        private const int BatchSize = 20;
        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(60);

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BasketOutboxProcessorJob> _logger;

        public BasketOutboxProcessorJob(IServiceScopeFactory serviceScopeFactory, ILogger<BasketOutboxProcessorJob> logger)
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

        
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var _dbSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

            var messages = await _dbSession.Query<BasketOutboxMessage>()
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

            await _dbSession.SaveChangesAsync(cancellationToken);
        }
    }
}
