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
using Payment.Infrastructure.Data.Extensions;
using Payment.Infrastructure.Extensions;    
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/payments/{id}", async (Guid id, IPaymentService paymentService, CancellationToken cancellationToken) =>
{
    var payment = await paymentService.GetPaymentTransactionById(id, cancellationToken);
    if (payment == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new PayWithCreditCardResponse(
        payment.Id.Value,
        payment.Status.ToString()));
});

app.MapPost("/payments/pay/{id}", async (Guid id, PayWithCreditCardRequest request, IPaymentService paymentService, CancellationToken cancellationToken) =>
{


    var providerPaymentRequest = new PaymentRequest
    {
        Amount = request.Amount,
        ProviderCode = "BankAnt",
        PaymentMethod = PaymentMethod.CreditCard,
        ProviderName = "BankAnt",
        CreditCardData = new CreditCardData
        {
            CardNumber = request.CardNumber,
            ExpiryMonth = request.Expiration,
            ExpiryYear = request.Expiration,
            Cvv = request.Cvv
        }
    };

    var paymentResult = await paymentService.ProcessPayment(providerPaymentRequest, cancellationToken);

    if(paymentResult.IsSuccess)
    {
        return Results.Ok(new PayWithCreditCardResponse(id,"Succeeded"));
    }
    else
    {
        return Results.BadRequest(new PayWithCreditCardResponse(id,"Failed"));
    }




})
.WithName("PayWithCreditCard")
.WithOpenApi();

app.Run();




