namespace Payment.API.Models
{
    public sealed record PayWithCreditCardResponse(
      Guid PaymentTransactionId,
      string Status);
}
