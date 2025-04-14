using System.ComponentModel.DataAnnotations;
using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; }
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
        [Required]
        public Seller Seller { get; set; }
        public List<Review> Reviews { get; set; }

        public void AddReview (Review review)
        {
            Reviews.Add (review);
        }
    }
}
