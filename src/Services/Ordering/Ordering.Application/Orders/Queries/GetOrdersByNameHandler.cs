

using Ordering.Application;  

namespace Ordering.Application.Orders.Queries
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orderList = await dbContext.Orders
                  .Include(o => o.ShippingAddress)
                  .Include(o => o.BillingAddress)
                  .Include(o => o.Payment)
                  .Include(o => o.OrderItems)
                  .AsNoTracking()
                  .Where(o => o.OrderName.Value.Contains(query.Name))
                  .OrderBy(o => o.OrderName)
                  .ToListAsync(cancellationToken);

            var orderDtos = orderList.ToOrderDtos();

            return new GetOrdersByNameResult(orderDtos.AsReadOnly());
        }

        
    }
}
