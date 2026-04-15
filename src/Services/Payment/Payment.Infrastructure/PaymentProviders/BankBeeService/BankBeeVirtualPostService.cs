using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using Payment.Domain.Enums;

namespace Payment.Infrastructure.PaymentProviders.BankBee
{
    public class BankBeeVirtualPostService : IPaymentProvider
    {
        public static List<PaymentMethod> SupportedPaymentMethods = new List<PaymentMethod>
        {
           
            PaymentMethod.BankTransfer
        };

        public string Name => "BankBee";

        public bool CanHandle(PaymentMethod method)
        {
            return SupportedPaymentMethods.Contains(method);
        }


        public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
