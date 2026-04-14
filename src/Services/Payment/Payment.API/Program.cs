using BuildingBlocks.Messaging.MassTransit;
using Payment.API.Models;
using Payment.Application.Data;
using Payment.Application.Dtos;
using Payment.Application.Extensions;
using Payment.Application.EventHandlers.Integration;
using Payment.Application.Interfaces;
using Payment.Domain.Enums;
using Payment.Domain.ValueObjects;  
using Payment.Infrastructure.Data;
using Payment.Infrastructure.Extensions;    
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddOrchestrationMessageBroker(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/payments/{id}", async (Guid id, IPaymentDbContext dbContext, CancellationToken cancellationToken) =>
{
    var payment = await dbContext.PaymentTransactions.FindAsync(PaymentTransactionId.Of(id), cancellationToken);
    if (payment == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new PayWithCreditCardResponse(
        payment.Id.Value,
        payment.OrderId.Value,
        payment.Status.ToString()));
});

app.MapPost("/payments/pay/{id}", async (Guid id, PayWithCreditCardRequest request, PaymentDbContext dbContext, IPaymentProvider provider, CancellationToken cancellationToken) =>
{

  

    var payment = await dbContext.PaymentTransactions.FindAsync(PaymentTransactionId.Of(id), cancellationToken);
    if (payment == null)
    {
        return Results.NotFound();
    }


    var providerPaymentRequest = new ProviderPaymentRequest
    {
        Amount = request.Amount,
        ProviderCode = "BankAnt",
        CreditCardData = new CreditCardData
        {
            CardNumber = request.CardNumber,
            ExpiryMonth = request.Expiration,
            ExpiryYear = request.Expiration,
            Cvv = request.Cvv
        }
    };

    var providerPaymentResult = await provider.ProcessPayment(providerPaymentRequest);

    if (providerPaymentResult == null)
    {
        payment.MarkFailed("Failed to process payment with the provider.", string.Empty);
        
    }

    if (!providerPaymentResult.IsSuccess)
    {
        payment.MarkFailed(providerPaymentResult.ErrorMessage, providerPaymentResult.ExternalTransactionId);
      

    }

    payment.MarkSucceeded(providerPaymentResult.ExternalTransactionId);
  

    dbContext.PaymentTransactions.Update(payment);

    await dbContext.SaveChangesAsync(cancellationToken);

    if (payment.Status == PaymentStatus.Failed)
    {
        return Results.BadRequest(new PayWithCreditCardResponse(
            payment.Id.Value,
            payment.OrderId.Value,
            payment.Status.ToString()));
    }

    return Results.Ok(new PayWithCreditCardResponse(
        payment.Id.Value,
        payment.OrderId.Value,
        payment.Status.ToString()));

})
.WithName("PayWithCreditCard")
.WithOpenApi();

app.Run();




