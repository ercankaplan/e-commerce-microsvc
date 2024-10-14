﻿
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{

    //public record DeleteProductRequest(Guid Id);

    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",async (ISender sender, Guid id) => 
            { 
                var command = new DeleteProductCommand(id);

                var result = await sender.Send(command);

                return Results.Ok(result.Adapt<DeleteProductResult>());
            })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Product")
                .WithDescription("Delete Product");
        }
    }
}
