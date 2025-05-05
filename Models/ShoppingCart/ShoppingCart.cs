using Tradie.Models.Products;

namespace Tradie.Models.ShoppingCart
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = new List<CartItem>();
        }
        public int Id { get; set; }
		//public List<CartItem> Items { get; private set; }
		public List<CartItem> Items { get; set; } = new List<CartItem>();
		public decimal Subtotal { get; set; }
		public decimal DeliveryFee { get; set; }
		public decimal Total { get; set; }
		public void AddItem(Product product, int quantity) {  }
        public void RemoveItem(int productId) {  }
        public void Clear() { Items.Clear(); }

        public List<CartItem> GetItems() { return Items; }
    }
}
