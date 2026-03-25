using Ordering.Application.Orders.Commands;

namespace Ordering.API.Endpoints
{
    /// <summary>
    /// Represents a response after deleting an existing order.
    /// </summary>
    public record DeleteOrderResponse(bool IsSuccess);

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteOrderCommand(id);

                var deleteOrderResult = await sender.Send(command);

                var response = new DeleteOrderResponse(deleteOrderResult.IsSuccess);

                return Results.Ok(response);
            })
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Deletes an existing order.")
            .WithDescription("Deletes an existing order by order identifier.");
        }
    }
}