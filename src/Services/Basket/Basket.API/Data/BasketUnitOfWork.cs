using Marten;

namespace Basket.API.Data
{
    public class BasketUnitOfWork(IDocumentSession dbSession) : IBasketUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => dbSession.SaveChangesAsync(cancellationToken);
    }
}