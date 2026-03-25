using Ordering.Application.Orders.Commands;

namespace Ordering.API.Endpoints
{
    /// <summary>
    /// Represents a request to update an existing order.
    /// </summary>
    public record UpdateOrderRequest(OrderDto Order);

    public record UpdateOrderResponse(bool IsSuccess);

    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var command = new UpdateOrderCommand(request.Order);

                var updateOrderResult = await sender.Send(command);

                var response = new UpdateOrderResponse(updateOrderResult.IsSuccess);

                return Results.Ok(response);
            })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Updates an existing order.")
            .WithDescription("Updates an existing order based on the provided order details.");
        }
    }
}