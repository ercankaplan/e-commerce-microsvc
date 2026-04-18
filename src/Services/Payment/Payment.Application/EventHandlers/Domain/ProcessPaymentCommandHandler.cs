using BuildingBlocks.CQRS;
using Payment.Application.Data;
using Payment.Domain.Enums;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;

namespace Payment.Application.EventHandlers.Domain
{

    public record ProcessPaymentCommand(Guid OrderId, Guid UserId, decimal Amount) : ICommand<ProcessPaymentResult>;

    public record ProcessPaymentResult(Guid PaymentTransactionId);

    public class ProcessPaymentCommandHandler(IPaymentDbContext dbContext) : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
    {
        public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
           

            var newPaymentTransaction = PaymentTransaction.Create(
                PaymentTransactionId.Of(Guid.NewGuid()),
                request.OrderId,
                request.Amount,
                "USD",
                PaymentMethod.CreditCard,
                PaymentType.Payment
            );
           

            dbContext.PaymentTransactions.Add(newPaymentTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProcessPaymentResult(newPaymentTransaction.Id.Value);    
         

        }
    }
}
