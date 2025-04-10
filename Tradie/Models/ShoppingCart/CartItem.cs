using Tradie.Models.Products;

namespace Tradie.Models.ShoppingCart
{    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAtAddition { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
