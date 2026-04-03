namespace Basket.API.Data
{
    public interface IBasketOutboxMessageRepository
    {
        Task<BasketOutboxMessage> AddMessage(BasketOutboxMessage message, CancellationToken cancellationToken = default);
    }
}
