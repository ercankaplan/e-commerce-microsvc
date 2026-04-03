using BuildingBlocks.Messaging.Events;

namespace Basket.API.Dtos
{
    public record BasketCheckoutDto
    {
        public string UserName { get; init; } = default!;

        public Guid CustomerId { get; init; } = default!;

        public decimal TotalPrice { get; init; } = default!;

        // Shipping Address and Billing Address can be added here as well

        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string AddressLine { get; init; } = default!;
        public string Country { get; init; } = default!;
        public string State { get; init; } = default!;
        public string ZipCode { get; init; } = default!;

        //Payment details can also be added here

        public string CardName { get; init; } = default!;
        public string CardNumber { get; init; } = default!;
        public string Expiration { get; init; } = default!;
        public string CVV { get; init; } = default!;
        public int PaymentMethod { get; init; } = default!;
    }

}
