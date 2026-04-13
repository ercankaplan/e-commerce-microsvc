using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Enums
{
    public enum PaymentMethod
    {
        Unknown = 0,
        CreditCard = 1,
        BankTransfer = 2,
        Wallet = 3
    }
}
