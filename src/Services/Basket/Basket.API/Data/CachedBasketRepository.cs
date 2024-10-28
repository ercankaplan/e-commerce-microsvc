using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {


        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;


            var basket = await repository.GetBasket(userName, cancellationToken);

            if (basket is not null)
                await cache.SetStringAsync(userName, System.Text.Json.JsonSerializer.Serialize(basket), cancellationToken);

            return basket;

        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var storedBasket = await repository.StoreBasket(basket, cancellationToken);

            if (storedBasket is not null)
                await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(storedBasket), cancellationToken);

            return storedBasket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var isDeleted = await repository.DeleteBasket(userName, cancellationToken);

            if(isDeleted)
                await cache.RemoveAsync(userName, cancellationToken);

            return isDeleted;
        }
    }
}
