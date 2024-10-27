



namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);

    public record StoreBasketResponse(string UserName);
  
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) => {
            
                
                var command = request.Adapt<StoreBasketCommand>();

                var result =  await sender.Send(command);

                if (result != null)
                {
                    var response = result.Adapt<StoreBasketResponse>();

                    return Results.Created($"/basket/{response.UserName}",response);
                }

                return Results.BadRequest();
          
            })
                 .WithName("StoreBasketByUserName")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem(StatusCodes.Status404NotFound)
            .WithSummary("Store Basket By UserName")
            .WithDescription("Store Basket By UserName");
        }
    }
}
