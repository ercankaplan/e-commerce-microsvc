
using Basket.API.Data;
using Discount.Grpc;


namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);
    


    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {

        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        }

    }

    internal class StoreBasketCommandHandler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountService) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            await DiscountCart(command.Cart, cancellationToken);

            var basket = await basketRepository.StoreBasket(command.Cart, cancellationToken);

            //TODO update cache

            return new StoreBasketResult(basket.UserName);
        }

     
        public async Task DiscountCart(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountService.GetDiscountAsync(new GetDiscountRequest() { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
