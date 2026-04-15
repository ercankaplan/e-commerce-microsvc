namespace Catalog.API.Models
{
    public class ReservedProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
    }
}
