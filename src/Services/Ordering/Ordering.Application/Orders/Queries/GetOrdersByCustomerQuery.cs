using System.Collections.ObjectModel;

namespace Ordering.Application.Orders.Queries
{
    public record GetOrdersByCustomerQuery(Guid CustomerId): IQuery<GetOrdersByCustomerResult>;

    public record GetOrdersByCustomerResult(ReadOnlyCollection<OrderDto> Orders);
    
}
