using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tradie.Models.Orders;
using Tradie.Models.Users;

namespace Tradie.Models.Products
{
	public class Product
	{
		public Product()
		{
			Reviews = new List<Review>();
			OrderItems = new List<OrderItem>();
			DateAdded = DateTime.UtcNow;
		}

		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "La descripción es obligatoria.")]
		public string Description { get; set; } = string.Empty;

		[Range(0.01, 1000000)]
		public decimal Price { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int? CategoryId { get; set; }  // nullable FK
		public Category? Category { get; set; }

		public string? Subcategory { get; set; }
		public string? ImageUrl { get; set; }
		public int Stock { get; set; }
		public int SellerId { get; set; }
		public DateTime DateAdded { get; set; }
		public Seller? Seller { get; set; }
		public List<Review> Reviews { get; set; }
		public List<OrderItem> OrderItems { get; set; }

		public void AddReview(Review review)
		{
			Reviews.Add(review);
		}

		public decimal? DiscountPercentage { get; set; }

		public decimal? DiscountedPrice
		{
			get
			{
				if (DiscountPercentage.HasValue)
				{
					return Price - (Price * DiscountPercentage.Value / 100);
				}
				return null;
			}
		}
	}
}
