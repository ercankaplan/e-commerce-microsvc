using Payment.Application.Dtos;
using Payment.Application.Interfaces;

namespace Payment.Infrastructure.PaymentProviders.BankBee
{
    public class BankBeeVirtualPost : IPaymentProvider
    {
        public Task<ProviderPaymentResult> ProcessPayment(ProviderPaymentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
