
using Ordering.Application.Orders.Commands;

namespace Ordering.API.Endpoints
{

    /// <summary>
    /// Represents a request to create a new order.
    /// </summary>
    
    public record CreateOrderRequest(OrderDto Order);

    public record CreateOrderResponse(Guid OrderId);


    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = new CreateOrderCommand(request.Order);

                var createOrderResult = await sender.Send(command);

                var response = new CreateOrderResponse(createOrderResult.Id);

                return Results.Created($"/orders/{createOrderResult.Id}", response);
            })
             .WithName("CreateOrder")
             .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Creates a new order.")
             .WithDescription("Creates a new order based on the provided order details.");
             
        }
    }
}
