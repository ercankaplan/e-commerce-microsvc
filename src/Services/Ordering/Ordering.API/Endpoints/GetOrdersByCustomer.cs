using Ordering.Application.Orders.Queries;
using System.Collections.ObjectModel;

namespace Ordering.API.Endpoints
{
    /// <summary>
    /// Represents a response containing orders filtered by customer identifier.
    /// </summary>
    public record GetOrdersByCustomerResponse(ReadOnlyCollection<OrderDto> Orders);

    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetOrdersByCustomerQuery(customerId);

                var result = await sender.Send(query, cancellationToken);

                var response = new GetOrdersByCustomerResponse(result.Orders);

                return Results.Ok(response);
            })
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets orders by customer.")
            .WithDescription("Gets orders filtered by customer identifier.");
        }
    }
}