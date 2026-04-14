using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Dtos
{
    public class ProviderPaymentRequest
    {
        public string ProviderCode { get; set; }
        public decimal Amount { get; set; }
        public string ProviderName { get; set; }
        public CreditCardData CreditCardData { get; set; }


    }

    public class CreditCardData
    {
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvv { get; set; }
    }
}
