using BuildingBlocks.CQRS;
using Payment.Application.Commands;
using Payment.Application.Data;
using Payment.Application.Dtos;
using Payment.Application.Interfaces;
using Payment.Domain.Enums;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.EventHandlers.Domain
{
    public class ProcessPaymentCommandHandler(IPaymentDbContext dbContext) : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
    {
        public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
           

            var newPaymentTransaction = PaymentTransaction.Create(
                PaymentTransactionId.Of(Guid.NewGuid()),
                request.OrderId,
                request.Amount,
                "USD",
                PaymentMethod.CreditCard
            );
           

            dbContext.PaymentTransactions.Add(newPaymentTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProcessPaymentResult(newPaymentTransaction.Id.Value);    
         

        }
    }
}
