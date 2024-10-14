

namespace Catalog.API.Products.CreateProduct
{
    //Application Logic Layer

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler (IDocumentSession docSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //logic to create product

            //create entity
            var product = new Product
            {
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Name = command.Name,
            };

            //save object

            docSession.Store(product);

            await docSession.SaveChangesAsync(cancellationToken);

            //return CreateProductResult
            return new CreateProductResult(product.Id);
        }
    }

    /* 
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :IRequest<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //logic to create product
            throw new NotImplementedException();
        }
    }

    */
}
