using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class UserProfileMainPage
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string? SellerResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
