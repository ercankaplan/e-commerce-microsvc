namespace Basket.API.Models
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; } = default!;

        public string Color { get; set; } = default!;

        public decimal Price { get; set; } = default!;

        public Guid Id { get; set; } = default!;

        public string ProductName { get; set; } = default!;
    }

    //public class Price
    //{
    //    public decimal OrginalPrice { get; set; } = default!;
    //    public decimal DiscountAmount { get; set; }
    //    public decimal Price { get; set; }
    //}

}
