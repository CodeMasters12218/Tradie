using System.ComponentModel.DataAnnotations;
using Tradie.Models.Orders;
using Tradie.Models.UserProfile;
using Tradie.Models.Users;

namespace Tradie.Models.Products
{
	public class ProductManagement
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[StringLength(100)]
		public string? Name { get; set; }
		[Required(ErrorMessage = "La descripción es obligatoria.")]
		public string? Description { get; set; }
		[Range(0.01, 1000000)]
		public string? Subcategory { get; set; }
		public decimal Price { get; set; }
		[Range(0, 100, ErrorMessage = "El porcentaje de descuento debe estar entre 0 y 100.")]
		public decimal? DiscountPercentage { get; set; } // nullable to allow non-discounted products

		[Required]
		public Seller? Seller { get; set; }
		public int SellerId { get; set; }
		public string? ImageUrl { get; set; }
		public int Stock { get; set; }

		public List<UserProfileMainPageModel>? Reviews { get; set; }

		public List<OrderItem>? OrderItems { get; set; }

		public void AddReview(UserProfileMainPageModel review)
		{
			if (Reviews == null)
				Reviews = new List<UserProfileMainPageModel>();

			Reviews.Add(review);
		}
	}
}
