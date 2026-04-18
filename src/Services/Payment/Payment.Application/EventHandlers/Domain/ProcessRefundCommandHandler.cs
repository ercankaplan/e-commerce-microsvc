using BuildingBlocks.CQRS;
using Payment.Application.Data;
using Payment.Domain.Enums;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;

namespace Payment.Application.EventHandlers.Domain
{

    public record ProcessRefundCommand(Guid OrderId, Guid UserId, decimal Amount, Guid ParentPaymentId) : ICommand<ProcessRefundResult>;

    public record ProcessRefundResult(Guid PaymentTransactionId);

    public class ProcessRefundCommandHandler(IPaymentDbContext dbContext) : ICommandHandler<ProcessRefundCommand, ProcessRefundResult>
    {
        public async Task<ProcessRefundResult> Handle(ProcessRefundCommand request, CancellationToken cancellationToken)
        {
           

            var newPaymentTransaction = PaymentTransaction.Create(
                PaymentTransactionId.Of(Guid.NewGuid()),
                request.OrderId,
                request.Amount,
                "USD",
                PaymentMethod.CreditCard,
                PaymentType.Refund,
                request.ParentPaymentId
            );
           

            dbContext.PaymentTransactions.Add(newPaymentTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProcessRefundResult(newPaymentTransaction.Id.Value);    
         

        }
    }
}
