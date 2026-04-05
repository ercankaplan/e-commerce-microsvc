using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Product> Products { get; }
        DbSet<InboxMessage> InboxMessages { get; }
        DbSet<InboxDeadLetterQueue> InboxDeadLetterQueues { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
