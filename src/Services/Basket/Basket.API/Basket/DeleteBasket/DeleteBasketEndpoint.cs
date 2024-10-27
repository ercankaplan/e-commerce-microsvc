﻿
namespace Basket.API.Basket.DeleteBasket
{

    //public record DeleteBasketRequest(string UserName);

    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{username}",async (ISender sender , string username) => { 
            
            
                var result = await sender.Send(new DeleteBasketCommand(username));

                return Results.Ok(result.Adapt<DeleteBasketResponse>());

            })
                  .WithName("DeleteBasket")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Basket")
                .WithDescription("Delete Basket");
        }
    }
}