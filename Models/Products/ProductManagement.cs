using System.ComponentModel.DataAnnotations;
using Tradie.Models.Users;
using Tradie.Models.Products;
using Tradie.Models.Orders;
using Tradie.Models.UserProfile;

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
        [Required]
        public Seller? Seller { get; set; }
        public int SellerId { get; set; }
		public string? ImageUrl { get; set; }
		public int Stock { get; set; }

		public List<UserProfileMainPageModel>? Reviews { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

        public void AddReview (UserProfileMainPageModel review)
        {
            Reviews.Add (review);
        }
    }
}
