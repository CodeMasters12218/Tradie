using Tradie.Models.Orders;
using Tradie.Models.Payments;
using Tradie.Models.Products;
using Tradie.Models.ShoppingCart;

namespace Tradie.Models.Users
{
	public class Customer : User
	{
		public Customer()
		{
			Role = UserRole.Customer;
			ShoppingCart = new Tradie.Models.ShoppingCart.ShoppingCart();
			Wishlist = new Tradie.Models.Wishlist.Wishlist();
			Orders = new List<Order>();
			Reviews = new List<Review>();
		}

		public Tradie.Models.ShoppingCart.ShoppingCart ShoppingCart { get; private set; }
		public Tradie.Models.Wishlist.Wishlist Wishlist { get; private set; }


		public List<Order> Orders { get; private set; }
		public List<Review> Reviews { get; set; }

		public void BrowseProducts(Category category) { /* ... */ }

		public List<Product> SearchProducts(string query)
		{
			return new List<Product>();
		}

		public void AddToCart(Product product, int quantity, decimal priceAtAddition)
		{
			ShoppingCart.AddItem(product, quantity, priceAtAddition);
		}

		public Order Checkout(PaymentMethod paymentMethod)
		{
			int id = 0;
			int customerId = 0;
			DateTime orderDate = DateTime.Now;
			OrderStatus status = OrderStatus.Procesado;
			List<CartItem> cartItems = ShoppingCart.GetItems();

			List<OrderItem> items = cartItems.Select(ci =>
				new OrderItem(
					ci.ProductId,
					ci.ProductName ?? "Unnamed Product",
					ci.PriceAtAddition,
					ci.Quantity
				)).ToList();


			var order = new Order(id, customerId, orderDate, status, items);
			Orders.Add(order);
			ShoppingCart.Clear();
			return order;
		}
		public void WriteProductReview(Product product, Review review)
		{
			// Set the customer ID
			review.CustomerId = this.Id;
			// Add to product reviews
			product.AddReview(review);
			// Add to customer reviews
			Reviews.Add(review);
		}
	}
}
