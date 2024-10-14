using Catalog.API.Products.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProductById
{
    //Presentation API Layer

    public record GetProductByIdRequest
    {
        [FromQuery]
        Guid Id { get; set; }
    }
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (ISender sender, Guid id) =>
            {
                var query = new GetProductByIdQuery() { Id = id };

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);

            }).WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id")
            ;
        }
    }



}
