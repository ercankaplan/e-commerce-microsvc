
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");

        }
    }

    internal class DeleteProductCommandHandler(IDocumentSession dbSession) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            //logger.LogInformation("DeleteProductCommandHandler called with {@Query}", command);

            var entity = await dbSession.LoadAsync<Product>(command.Id, cancellationToken);

            if (entity == null) { throw new ProductNotFoundException(command.Id); }


            dbSession.Delete(entity);

            await dbSession.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
