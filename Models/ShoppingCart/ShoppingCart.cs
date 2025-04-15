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
        public List<CartItem> Items { get; private set; }

        public void AddItem(Product product, int quantity) {  }
        public void RemoveItem(int productId) {  }
        public void Clear() { Items.Clear(); }

        public List<CartItem> GetItems() { return Items; }
    }
}
