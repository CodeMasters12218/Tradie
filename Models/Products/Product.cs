using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Seller Seller { get; set; }
        public List<Review> Reviews { get; set; }

        public void AddReview (Review review)
        {

        }
    }
}
