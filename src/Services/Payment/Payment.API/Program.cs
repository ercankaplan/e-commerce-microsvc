using Payment.Domain.Enums;
using Payment.Domain.Models;
using Payment.Domain.ValueObjects;
using Payment.Infrastructure.Data;
using Payment.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/payments/pay-with-credit-card", async (PayWithCreditCardRequest request, PaymentDbContext dbContext, CancellationToken cancellationToken) =>
{
    var payment = PaymentTransaction.Create(
        PaymentTransactionId.Of(Guid.NewGuid()),
        request.OrderId,
        request.Amount,
        request.Currency,
        PaymentMethod.CreditCard);

    if (request.CardNumber.Length < 12)
    {
        payment.MarkFailed("Card was declined.");
    }
    else
    {
        payment.MarkSucceeded($"pi_{Guid.NewGuid():N}");
    }

    await dbContext.PaymentTransactions.AddAsync(payment, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    if (payment.Status == PaymentStatus.Failed)
    {
        return Results.BadRequest(new PayWithCreditCardResponse(
            payment.Id.Value,
            payment.OrderId.Value,
            payment.Status.ToString(),
            payment.ExternalTransactionId,
            payment.FailureReason));
    }

    return Results.Ok(new PayWithCreditCardResponse(
        payment.Id.Value,
        payment.OrderId.Value,
        payment.Status.ToString(),
        payment.ExternalTransactionId,
        payment.FailureReason));
})
.WithName("PayWithCreditCard")
.WithOpenApi();

app.Run();

public sealed record PayWithCreditCardRequest(
    Guid OrderId,
    decimal Amount,
    string Currency,
    string CardHolderName,
    string CardNumber,
    string Expiration,
    string Cvv);

public sealed record PayWithCreditCardResponse(
    Guid PaymentId,
    Guid OrderId,
    string Status,
    string? ExternalTransactionId,
    string? FailureReason);
