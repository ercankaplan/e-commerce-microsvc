using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.PaymentProviders.Stripe
{
    public class Stripe : IPaymentProvider
    {
        public Task<ProviderPaymentResult> ProcessPayment(ProviderPaymentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
