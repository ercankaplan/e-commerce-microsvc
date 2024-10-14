
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductCommandHandler(IDocumentSession dbSession,ILogger<DeleteProductCommand> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler called with {@Query}", command);

            var entity = await dbSession.LoadAsync<Product>(command.Id, cancellationToken);

            if (entity == null) { throw new ProductNotFoundExceptions(); }


            dbSession.Delete(entity);

            await dbSession.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
