using System.ComponentModel.DataAnnotations;
using Tradie.Models.Users;
using Tradie.Models.Products;
using Tradie.Models.Orders;

namespace Tradie.Models.Products
{
	public class Product
	{
		public Product()
		{
			Reviews = new List<Review>();
			OrderItems = new List<OrderItem>();
		}

		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "La descripción es obligatoria.")]
		public string Description { get; set; } = string.Empty;

		[Range(0.01, 1000000)]
		public decimal Price { get; set; }

		public string? Subcategory { get; set; }
		public string? ImageUrl { get; set; }
		public int Stock { get; set; }
		public int SellerId { get; set; }
		public Seller? Seller { get; set; }
		public List<Review> Reviews { get; set; }
		public List<OrderItem> OrderItems { get; set; }

		public void AddReview(Review review)
		{
			Reviews.Add(review);
		}
	}
}
