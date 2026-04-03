

namespace Basket.API.Basket.CheckoutBasket
{

  
    public record CheckoutBasketRequest(BasketCheckoutDto basketCheckoutDto);

    public record CheckoutBasketResponse(bool isSuccess);

    public class CheckoutBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (ISender sender, CheckoutBasketRequest request) =>
            {
                var command = new CheckoutBasketCommand(request.basketCheckoutDto);
                var result = await sender.Send(command);
                if (result != null)
                {
                    var response = result.Adapt<CheckoutBasketResponse>();
                    return Results.Ok(response);
                }
                return Results.BadRequest();
            })
            .WithName("CheckoutBasketByUserName")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket")
            .WithDescription("Checkout Basket");
        }
    }
}
