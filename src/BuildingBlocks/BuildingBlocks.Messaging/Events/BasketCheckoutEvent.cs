namespace BuildingBlocks.Messaging.Events
{
    public record BasketCheckoutEvent //: IntegrationEvent
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
        public string PaymentMethod { get; init; } = default!;

        public List<BasketCheckoutOrderItem> Items { get; init; } = new();
    }

    public record BasketCheckoutOrderItem(int Quantity,decimal Price);
}
