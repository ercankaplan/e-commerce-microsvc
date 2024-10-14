
using Catalog.API.Products.GetProductByCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand (Guid Id, string? Name, List<string> Category, string? Description, string? ImageFile, decimal Price) : ICommand<UpdateProductResult>;
                                    
        
    public record UpdateProductResult(bool IsSuccess);



    internal class UpdateProductCommandHandler(IDocumentSession dbSession,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductHandler called with {@Query}", command);

            var entity = await dbSession.LoadAsync<Product>(command.Id,cancellationToken);

            if (entity == null) { throw new ProductNotFoundExceptions(); }

            //var newEntity = command.Adapt<Product>();

            entity.Name = command.Name;
            entity.Category = command.Category;
            entity.Description = command.Description;
            entity.ImageFile = command.ImageFile;
            entity.Price = command.Price;

            dbSession.Update(entity);

            await dbSession.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
