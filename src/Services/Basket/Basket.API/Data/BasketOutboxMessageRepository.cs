
using Marten;

namespace Basket.API.Data
{
    public class BasketOutboxMessageRepository(IDocumentSession dbSession) : IBasketOutboxMessageRepository
    {
        public async Task<BasketOutboxMessage> AddMessage(BasketOutboxMessage message, CancellationToken cancellationToken = default)
        {
            dbSession.Store<BasketOutboxMessage>(message);

            //await dbSession.SaveChangesAsync(cancellationToken);

            return message;
        }
    }
}
