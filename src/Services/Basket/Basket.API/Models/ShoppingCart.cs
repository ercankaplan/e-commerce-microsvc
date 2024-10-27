using Marten.Schema;

namespace Basket.API.Models
{
    public class ShoppingCart
    {
        [Identity]
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        
        public decimal TotalPrice => Items.Sum(x => x.Price*x.Quantity);

        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string UserName)
        {
            this.UserName = UserName;
        }
    }
}
