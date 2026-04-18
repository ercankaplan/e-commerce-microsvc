using MediatR;
using Payment.Domain.Abstractions;
using Payment.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Events
{
    public sealed record PaymentFailedDomainEvent(
       PaymentTransactionId PaymentId,
       Guid OrderId,
       string Reason) : IDomainEvent;
}
