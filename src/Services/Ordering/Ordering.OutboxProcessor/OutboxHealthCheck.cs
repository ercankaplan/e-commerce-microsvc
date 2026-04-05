using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.OutboxProcessor
{
    public class OutboxHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,CancellationToken cancellationToken = default)
        {
            var unpublished = await _dbContext.OutboxMessages.CountAsync(m => m.ProcessedOnUtc == null, cancellationToken);
            
            return unpublished > 1000
                ? HealthCheckResult.Unhealthy($"Outbox backlog: {unpublished} messages")
                : HealthCheckResult.Healthy($"Outbox healthy: {unpublished} pending");
        }
    }
}
