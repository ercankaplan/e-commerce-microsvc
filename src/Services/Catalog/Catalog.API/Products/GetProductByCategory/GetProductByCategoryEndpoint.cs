

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProductByCategory
{

    public record GetProductByCategoryRequest:IRequest<GetProductByCategoryRequest>
    {
        [FromQuery]
       public string? Category { get; set; }
    }

    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.Map("/products/category/{category}", async (ISender sender, string category) =>
            {
                var request = new GetProductByCategoryRequest() { Category = category };

                var query = request.Adapt<GetProductByCategoryQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");

           
        }
    }
}
