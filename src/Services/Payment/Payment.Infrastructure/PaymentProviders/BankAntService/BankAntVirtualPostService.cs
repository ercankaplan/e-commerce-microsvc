using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using Payment.Domain.Enums;

namespace Payment.Infrastructure.PaymentProviders.BankAnt
{
    public class BankAntVirtualPostService : IPaymentProvider
    {

        public static List<PaymentMethod> SupportedPaymentMethods = new List<PaymentMethod>
        {
            PaymentMethod.CreditCard,
            PaymentMethod.BankTransfer
        };

        public string Name => "BankAnt";

        public bool CanHandle(PaymentMethod method)
        {
            return SupportedPaymentMethods.Contains(method);
        }


        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if(request.Amount <= 0)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Amount must be greater than zero."
                };
            }

            // Simulate a successful payment for demonstration purposes
            return new PaymentResult
            {
                IsSuccess = true,
                ExternalTransactionId = "transaction-id-123"
            };
        }
    }
}
