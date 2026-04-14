namespace Payment.API.Models
{
    public sealed record PayWithCreditCardResponse(
      Guid PaymentTransactionId,
      Guid OrderId,
      string Status);
}
