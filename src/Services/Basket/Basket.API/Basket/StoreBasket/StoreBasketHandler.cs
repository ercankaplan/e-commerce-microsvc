﻿
using Basket.API.Data;

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

    internal class StoreBasketCommandHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            ShoppingCart cart = command.Cart;

            var basket = await basketRepository.StoreBasket(cart, cancellationToken);

            //TODO update cache

            return new StoreBasketResult(basket.UserName);
        }
    }
}