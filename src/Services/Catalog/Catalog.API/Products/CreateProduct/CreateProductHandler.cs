



namespace Catalog.API.Products.CreateProduct
{
    //Application Logic Layer

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession docSession
        //,ILogger<CreateProductCommandHandler> logger
        /*,IValidator<CreateProductCommand> validator*/) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            //logger.LogInformation("CreateProductCommandHandler called with {@Query}", command);

            /*-----------------------------
             *      this block is takenover by Validation behavior middleware

                   var validationResult = await validator.ValidateAsync(command, cancellationToken);

                   if (!validationResult.IsValid)
                         throw new ValidationException(validationResult.Errors);
           *------------------------------*/


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
