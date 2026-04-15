using BuildingBlocks.Messaging.Events.Inventory;
using BuildingBlocks.Messaging.Events.Order;
using Catalog.API.Products.GetProductById;
using MassTransit;

namespace Catalog.API.Products.ReserveProduct
{
   
     public record ReserveProductCommand(Guid OrderId, Guid ProductId, int Quantity)
        : ICommand<ReserveProductResult>;

    public record ReserveProductResult(bool IsSuccess, Guid? Id);

    internal class ReserveProductHandler(IDocumentSession docSession, IPublishEndpoint publishEndpoint) : ICommandHandler<ReserveProductCommand, ReserveProductResult>
    {
        public async Task<ReserveProductResult> Handle(ReserveProductCommand command, CancellationToken cancellationToken)
        {


         
             var product = await docSession.Query<Product>().FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

            if (product == null)
            {
                await publishEndpoint.Publish(new IntEventOrderFailed() { OrderId = command.OrderId, Reason = $"Product with id {command.ProductId} not found" }, cancellationToken);
                return new ReserveProductResult(false, null);
            }
            //create entity
            var reserve = new ReservedProduct
            {
                Id = Guid.NewGuid(),
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                OrderId = command.OrderId,
                Price = product.Price
            };

            //save object

            docSession.Store(reserve);

            await docSession.SaveChangesAsync(cancellationToken);

            //return CreateProductResult
            await publishEndpoint.Publish(new IntEventInventoryReserved() { OrderId = command.OrderId }, cancellationToken);
            return new ReserveProductResult(true, reserve.Id);
        }
    }
}
