using Payment.Application.Dtos;
using Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPayment(PaymentRequest request, CancellationToken cancellationToken);
        Task<PaymentTransaction?> GetPaymentTransactionById(Guid id, CancellationToken cancellationToken);
    }
}
