using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(Microsoft.EntityFrameworkCore.DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries<IEntity>().ToList();

            var currentTime = DateTime.UtcNow;
            foreach (var entry in entries)
            {

                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    entry.Entity.CreatedAt = currentTime;
                    entry.Entity.CreatedBy = "system"; // You can replace "system" with the actual user identifier
                }

                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added || 
                     entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified || 
                     entry.HasChangedEntities())
                {
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = "system"; // You can replace "system" with the actual user identifier
                }

            }
        }
    }

    public static class EntityEntryExtensions
    {
        public static bool HasChangedEntities(this Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry) => 
            entry.References.Any(r =>
           r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Added ||
            r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Modified
            )
        );
    }
}
