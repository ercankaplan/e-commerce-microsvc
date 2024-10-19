using Catalog.API.Products.GetProduct;
using Catalog.API.Products.GetProductById;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery : IQuery<GetProductByCategoryResult>
    { 
        public string? Category {  get; set; }
    }

public record GetProductByCategoryResult(IEnumerable<Product> Products);



    internal class GetProductByCategoryHandler(IDocumentSession dbSession)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            //logger.LogInformation("GetProductByCategoryHandler called with {@Query}", query);

            var products = await dbSession.Query<Product>().
                Where(x=> x.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            if(products.Count == 0) { throw new ProductNotFoundException(query.Category); }

            return new GetProductByCategoryResult(products);
        }
    }



}
