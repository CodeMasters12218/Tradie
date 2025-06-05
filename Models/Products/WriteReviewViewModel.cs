using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class WriteReviewViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public ProductSummaryDto Product { get; set; }      // Id, ImageUrl, Name, Price, Quantity
        public int Quantity { get; set; }
        public int UserRating { get; set; }                 // Valoración que el usuario escogerá
        public string UserComment { get; set; }
    }

}
