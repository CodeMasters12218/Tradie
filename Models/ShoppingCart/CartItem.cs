using Tradie.Models.Products;

namespace Tradie.Models.ShoppingCart
{
	public class CartItem
	{
		public int Id { get; set; }

		public int ProductId { get; set; }
		public Product? Product { get; set; }

		public string? ProductName { get; set; } = string.Empty;
		public string? ImageUrl { get; set; }
		public string? CategoryName { get; set; }

		public int Quantity { get; set; }

		public decimal PriceAtAddition { get; set; }
		public string? Size { get; set; }
		public string? Color { get; set; }
		public string? Status { get; set; }

		public int ShoppingCartId { get; set; }
		public ShoppingCart? ShoppingCart { get; set; }
	}
}
