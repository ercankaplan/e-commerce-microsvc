

namespace Ordering.Application.Orders.Queries
{
    public class GetOrdersByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orderList = await dbContext.Orders
                  .Include(o => o.ShippingAddress)
                  .Include(o => o.BillingAddress)
                  .Include(o => o.Payment)
                  .Include(o => o.OrderItems)
                  .AsNoTracking()
                  .Where(o => o.CustomerId.Value == query.CustomerId)
                  .OrderBy(o => o.CreatedAt)
                  .ToListAsync(cancellationToken);

            var orderDtos = orderList.ToOrderDtos();

            return new GetOrdersByCustomerResult(orderDtos.AsReadOnly());
        }
    }
}