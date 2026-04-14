using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Data
{
    public interface IPaymentDbContext
    {
        DbSet<PaymentTransaction> PaymentTransactions { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
