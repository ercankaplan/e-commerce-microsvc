
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductByCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand (Guid Id, string? Name, List<string> Category, string? Description, string? ImageFile, decimal Price) : ICommand<UpdateProductResult>;
                                    
        
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                                .Length(2, 150).WithMessage("Must be between 2 and 150 characters");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession dbSession,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductHandler called with {@Query}", command);

            var entity = await dbSession.LoadAsync<Product>(command.Id,cancellationToken);

            if (entity == null) { throw new ProductNotFoundException(command.Id); }

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
