using Tradie.Models.Orders;
using Tradie.Models.Products;

namespace Tradie.Models.Users
{
    public class Customer : User
    {
        public Customer()
        {
            Role = UserRole.Customer;
            ShoppingCart = new ShoppingCart();
            Orders = new List<Order>();
        }
        public ShoppingCart ShoppingCart { get; private set; }
        public List<Order> Orders { get; private set; }

        public void BrowseProducts(Category category) { /* ... */ }

        public List<Product> SearchProducts(string query)
        {
            return new List<Product>();
        }

        public void AddToCart(Product product, int quantity)
        {
            ShoppingCart.AddItem(product, quantity);
        }
        public Order Checkout(PaymentMethod paymentMethod)
        {
            var order = new Order(this, ShoppingCart);
            Orders.Add(order);
            ShoppingCart.Clear();
            return order;
        }
        public void WriteProductReview(Product product, Review review)
        {
            product.AddReview(review);
        }
    }
}
