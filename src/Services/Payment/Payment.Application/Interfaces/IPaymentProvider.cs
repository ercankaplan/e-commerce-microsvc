using Payment.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interfaces
{
    public interface IPaymentProvider
    {
        public Task<ProviderPaymentResult> ProcessPayment(ProviderPaymentRequest request);
    }
}
