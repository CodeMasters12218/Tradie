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
		public string UserId { get; set; } = string.Empty;
		public User? User { get; set; }

		public List<CartItem> Items { get; set; } = new List<CartItem>();

		public decimal Subtotal => Items.Sum(i => i.PriceAtAddition * i.Quantity);
		public decimal DeliveryFee => Items.Any() ? 4.00m : 0.00m;
		public decimal Total => Subtotal + DeliveryFee;

		public void AddItem(Product product, int quantity)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));

			var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
			if (existingItem != null)
			{
				existingItem.Quantity += quantity;
			}
			else
			{
				Items.Add(new CartItem
				{
					ProductId = product.Id,
					ProductName = product.Name,
					Quantity = quantity,
					PriceAtAddition = product.Price,
					ImageUrl = product.ImageUrl,
					Size = "Default", // Customize based on your logic
					Color = "Default" // Customize based on your logic
				});
			}
		}

		public void RemoveItem(int productId) { }
		public void Clear() { Items.Clear(); }

		public List<CartItem> GetItems() { return Items; }
	}
}
