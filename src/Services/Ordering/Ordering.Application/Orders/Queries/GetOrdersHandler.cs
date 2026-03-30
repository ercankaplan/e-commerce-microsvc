
namespace Ordering.Application.Orders.Queries
{
    public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {

            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

            var orders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Skip((query.PaginationRequest.PageIndex - 1) * query.PaginationRequest.PageSize)
                .Take(query.PaginationRequest.PageSize)
                .OrderBy(o => o.Id)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(
                new PaginatedResult<OrderDto>(
                query.PaginationRequest.PageIndex,
                query.PaginationRequest.PageSize,
                totalCount,
                orders.ToOrderDtos())
                );

        }
    }
}
