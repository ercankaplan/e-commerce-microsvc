using Ordering.Application.Orders.Queries;
using System.Collections.ObjectModel;

namespace Ordering.API.Endpoints
{
    /// <summary>
    /// Represents a response containing orders filtered by order name.
    /// </summary>
    public record GetOrdersByNameResponse(ReadOnlyCollection<OrderDto> Orders);

    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/name/{name}", async (string name, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetOrdersByNameQuery(name);

                var result = await sender.Send(query, cancellationToken);

                var response = new GetOrdersByNameResponse(result.Orders);

                return Results.Ok(response);
            })
            .WithName("GetOrdersByName")
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets orders by name.")
            .WithDescription("Gets orders filtered by order name.");
        }
    }
}