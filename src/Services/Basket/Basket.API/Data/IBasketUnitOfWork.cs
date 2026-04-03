namespace Basket.API.Data
{
    public interface IBasketUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}