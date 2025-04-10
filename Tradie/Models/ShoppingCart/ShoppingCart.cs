using Tradie.Models.Products;

namespace Tradie.Models.ShoppingCart
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; }

        public void AddItem(Product product, int quantity) {  }
        public void RemoveItem(int productId) {  }
        public void Clear() { Items.Clear(); }
    }
}
