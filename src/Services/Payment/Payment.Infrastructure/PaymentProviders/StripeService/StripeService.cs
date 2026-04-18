using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using Payment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.PaymentProviders.Stripe
{
    public class StripeService : IPaymentProvider
    {
        public static List<PaymentMethod> SupportedPaymentMethods = new List<PaymentMethod>
        {
            PaymentMethod.Wallet
        };
        public string Name => "Stripe";

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
