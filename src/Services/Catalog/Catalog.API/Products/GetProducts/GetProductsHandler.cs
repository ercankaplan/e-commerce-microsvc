

namespace Catalog.API.Products.GetProduct
{

    public record GetProductsQuery : IQuery<GetProductsResult>;
   

    public record GetProductsResult(IEnumerable<Product> Products);


    internal class GetProductsQueryHandler(IDocumentSession dbSession, ILogger<GetProductsQueryHandler> logger) 
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler called with {@Query}", query);

            var products = await  dbSession.Query<Product>().ToListAsync(cancellationToken);

            return new GetProductsResult(products);

            
        }
    }
}
