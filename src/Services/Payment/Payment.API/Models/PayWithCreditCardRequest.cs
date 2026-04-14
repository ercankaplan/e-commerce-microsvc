namespace Payment.API.Models
{
    public sealed record PayWithCreditCardRequest(
     Guid PaymentTransactionId,
     decimal Amount,
     string Currency,
     string CardHolderName,
     string CardNumber,
     string Expiration,
     string Cvv);
}
