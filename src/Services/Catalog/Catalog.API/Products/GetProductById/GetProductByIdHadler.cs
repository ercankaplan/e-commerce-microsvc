using Catalog.API.Products.GetProduct;
using JasperFx.Core;

namespace Catalog.API.Products.GetProductById
{

    public record GetProductByIdQuery : IQuery<GetProductByIdResult>
    { 
        public Guid Id { get; set; }
    }


    public record GetProductByIdResult(Product Product);


  
    internal class GetProductByIdQueryHadler(IDocumentSession dbSession)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            //logger.LogInformation("GetProductByIdQueryHadler called with {@Query}", query);

            var product = await dbSession.LoadAsync<Product>(query.Id, cancellationToken);

            if(product is null)
                throw new ProductNotFoundException(query.Id); 

            return new GetProductByIdResult(product);
        }
    }


}
