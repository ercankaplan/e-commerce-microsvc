using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Dtos
{
    public class ProviderPaymentResult
    {
        public bool IsSuccess { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; } = string.Empty;
        public string? ExternalTransactionId { get; set; }
        public string? FailureReason { get; set; }
    }
}
