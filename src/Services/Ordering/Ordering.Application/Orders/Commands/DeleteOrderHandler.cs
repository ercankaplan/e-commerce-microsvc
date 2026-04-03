using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.Application.Orders.Commands
{
    public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderID = OrderId.Of(command.OrderId);
            var existingOrder = await dbContext.Orders.FindAsync([orderID], cancellationToken);
            if (existingOrder == null)
            {
                throw new OrderNotFoundException(orderID.Value);
            }

            dbContext.Orders.Remove(existingOrder);

            var result = await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(result > 0);
        }
    }
}
