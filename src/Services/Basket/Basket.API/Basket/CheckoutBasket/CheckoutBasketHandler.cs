
using Basket.API.Data;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MassTransit.Middleware;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;


    public record CheckoutBasketResult(bool IsSuccess);


    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto cannot be null.");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName cannot be empty.");

        }
    }

    public class CheckoutBasketHandler(IBasketRepository basketRepository, 
        IBasketOutboxMessageRepository basketOutboxMessageRepository,
        IBasketUnitOfWork basketUnitOfWork)//,IPublishEndpoint  publishEndpoint) 
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
            if (basket == null)
            {
                return new CheckoutBasketResult(false);
            }

            var eventMessage = command.BasketCheckoutDto.Adapt<IntEventBasketCheckout>();

            eventMessage = eventMessage with
            {
                TotalPrice = basket.Items.Sum(i => i.Price * i.Quantity),
                Items = basket.Items.Select(i => new BasketCheckoutOrderItem(i.Quantity, i.Price)).ToList()
            };

            //TODO : write event to outboxmessage table

           await basketOutboxMessageRepository.AddMessage(new BasketOutboxMessage
            {
                ContentType = "application/json",
                EventName = EventContracts.BasketCheckout.Name,
                EventType = typeof(IntEventBasketCheckout).AssemblyQualifiedName,
                EventVersion = EventContracts.BasketCheckout.Version,
                EnvelopeVersion = 1,
                Payload = System.Text.Json.JsonSerializer.Serialize(eventMessage),
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Metadata = null
            }, cancellationToken);

            //await publishEndpoint.Publish(eventMessage, cancellationToken);

            await basketRepository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

            await basketUnitOfWork.SaveChangesAsync(cancellationToken);

            return new CheckoutBasketResult(true);
        }


    }
}
