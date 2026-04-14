using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Commands
{
    public record ProcessPaymentCommand(Guid OrderId, Guid UserId, decimal Amount) : ICommand<ProcessPaymentResult>;

    public record ProcessPaymentResult(Guid PaymentTransactionId);

}
