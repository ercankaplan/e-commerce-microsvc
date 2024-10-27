
using Basket.API.Exceptions;
using Marten;

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession dbSession) : IBasketRepository
    {

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await dbSession.LoadAsync<ShoppingCart>(userName,cancellationToken);

            return basket is null ? throw new BasketNotFoundException("Basket", userName) : basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            dbSession.Store<ShoppingCart>(basket);

            await dbSession.SaveChangesAsync(cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            dbSession.Delete<ShoppingCart>(userName);

            await dbSession.SaveChangesAsync(cancellationToken);

            return true;
        }

       
    }
}
