using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class PendingReviewViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public ProductSummaryDto Product { get; set; }
        public int Quantity { get; set; }
        public int RatingByUser { get; internal set; }
        public string? CommentByUser { get; internal set; }
        public DateTime DateByUser { get; internal set; }
    }

}
