using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Queries;

namespace Ordering.API.Endpoints
{
    public record GetOrdersRequest([FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 10);

    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] GetOrdersRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetOrdersQuery(new PaginationRequest(request.PageIndex, request.PageSize));

                var result = await sender.Send(query, cancellationToken);

                var response = new GetOrdersResponse(result.Orders);

                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets orders with pagination.")
            .WithDescription("Gets orders using page index and page size query parameters.");
        }
    }
}