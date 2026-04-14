using Payment.Application.Dtos;
using Payment.Application.Interfaces;

namespace Payment.Infrastructure.PaymentProviders.BankAnt
{
    public class BankAntVirtualPost : IPaymentProvider
    {
        public async Task<ProviderPaymentResult> ProcessPayment(ProviderPaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if(request.Amount <= 0)
            {
                return new ProviderPaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Amount must be greater than zero."
                };
            }

            // Simulate a successful payment for demonstration purposes
            return new ProviderPaymentResult
            {
                IsSuccess = true,
                ExternalTransactionId = "transaction-id-123"
            };
        }
    }
}
