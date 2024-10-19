

using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{

    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
   

    public record GetProductsResult(IEnumerable<Product> Products);


    internal class GetProductsQueryHandler(IDocumentSession dbSession) 
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            //logger.LogInformation("GetProductsQueryHandler called with {@Query}", query);

            //var products = await  dbSession.Query<Product>().ToListAsync(cancellationToken);

            var products = await dbSession.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1 ,query.PageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);

            
        }
    }
}
