using Microsoft.EntityFrameworkCore;
using Payment.Application.Data;
using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;
using System.Threading;

namespace Payment.Application.Services
{
    public class PaymentService: IPaymentService
    {

        private readonly Dictionary<string, IPaymentProvider> _strategies;
        private readonly IPaymentDbContext _dbContext;

        public PaymentService(IEnumerable<IPaymentProvider> strategies, IPaymentDbContext dbContext)
        {
            _strategies = strategies.ToDictionary(s => s.Name, s => s);
            _dbContext = dbContext;
        }

        public async Task<PaymentTransaction?> GetPaymentTransactionById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.PaymentTransactions.FindAsync([PaymentTransactionId.Of(id)], cancellationToken);
        }

        public async Task<PaymentResult> ProcessPayment(PaymentRequest request,CancellationToken cancellationToken)
        {

            var payment = await _dbContext.PaymentTransactions.FindAsync([PaymentTransactionId.Of(request.Id)], cancellationToken);

            foreach (var pName in _strategies.Keys)
            {
                var strategy = _strategies[pName];

                if (strategy == null)
                {
                    continue;
                    //throw new InvalidOperationException($"No payment provider found for method {request.PaymentMethod}");
                }

                if (!strategy.CanHandle(request.PaymentMethod))
                    continue;

                var providerPaymentResult = await strategy.ProcessPaymentAsync(request);

                if (providerPaymentResult == null)
                {
                    payment.MarkFailed("Failed to process payment with the provider.", string.Empty);

                }

                if (!providerPaymentResult.IsSuccess)
                {
                    payment.MarkFailed(providerPaymentResult.ErrorMessage, providerPaymentResult.ExternalTransactionId);


                }

                payment.MarkSucceeded(providerPaymentResult.ExternalTransactionId);


                _dbContext.PaymentTransactions.Update(payment);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return providerPaymentResult;
            }

            return new PaymentResult() { IsSuccess = false, ErrorMessage = "Payment provider not found" };

        }
    }
}
