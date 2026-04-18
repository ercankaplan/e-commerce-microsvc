using Payment.Application.Dtos;
using Payment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interfaces
{
    public interface IPaymentProvider
    {
        public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);

        public bool CanHandle(PaymentMethod method);

        public string Name { get; }
    }
}
