using Payment.Domain.Enums;
using Payment.Domain.Events;
using Payment.Domain.Abstractions;

namespace Payment.Domain.Models
{
    public class PaymentTransaction : Aggregate<PaymentTransactionId>
    {
        public PaymentTransactionId Id { get; private set; } = default!;
        public OrderId OrderId { get; private set; } = default!;
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? ExternalTransactionId { get; private set; }
        public string? FailureReason { get; private set; }

        private PaymentTransaction() { }

        public static PaymentTransaction Create(
            PaymentTransactionId id,
            Guid orderId,
            decimal amount,
            string currency,
            PaymentMethod paymentMethod)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }

            return new PaymentTransaction
            {
                Id = id,
                OrderId = OrderId.Of(orderId),
                Amount = amount,
                Currency = currency.Trim().ToUpperInvariant(),
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Pending
            };
        }

        public void MarkSucceeded(string externalTxnId)
        {
            if (string.IsNullOrWhiteSpace(externalTxnId))
            {
                throw new ArgumentException("External transaction id is required.", nameof(externalTxnId));
            }

            if (Status == PaymentStatus.Succeeded)
            {
                return;
            }

            if (Status == PaymentStatus.Failed)
            {
                throw new InvalidOperationException("Failed payment cannot be marked as succeeded.");
            }

            Status = PaymentStatus.Succeeded;
            ExternalTransactionId = externalTxnId.Trim();
            FailureReason = null;

            AddDomainEvent(new PaymentSucceededDomainEvent(Id, OrderId.Value, ExternalTransactionId));
        }

        public void MarkFailed(string reason, string externalTransactionId)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Failure reason is required.", nameof(reason));
            }

            if (string.IsNullOrWhiteSpace(externalTransactionId))
            {
                throw new ArgumentException("External transaction id is required.", nameof(externalTransactionId));
            }

            if (Status == PaymentStatus.Failed)
            {
                return;
            }

            if (Status == PaymentStatus.Succeeded)
            {
                throw new InvalidOperationException("Succeeded payment cannot be marked as failed.");
            }

            Status = PaymentStatus.Failed;
            FailureReason = reason.Trim();
            ExternalTransactionId = externalTransactionId.Trim();

            AddDomainEvent(new PaymentFailedDomainEvent(Id, OrderId.Value, FailureReason));
        }

        public void MarkRefunded()
        {
            throw new NotSupportedException("Refund flow will be implemented in a future phase.");
        }
    }

  

  
}