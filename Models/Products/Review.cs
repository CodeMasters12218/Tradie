using Tradie.Models.Orders;
using Tradie.Models.Users;

namespace Tradie.Models.Products
{
	public class Review
	{
		public int Id { get; set; }
        public int OrderId { get; set; }
		public Order Order { get; set; }
        public int SellerId { get; set; }
		public int SellerRating { get; set; }
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
