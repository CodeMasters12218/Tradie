namespace Tradie.Models.Users
{
    public class UserReviewViewModel
    {
        public string OrderNumber { get; set; }
        public int OrderId { get; set; }
        public ProductSummaryDto Product { get; set; }  // Objeto que contenga ImageUrl, Name, Price, etc.
        public int Quantity { get; set; }
        public int RatingByUser { get; set; }          // Puntuación que el usuario dio
        public string CommentByUser { get; set; }
        public DateTime DateByUser { get; set; }
        public int? RatingBySeller { get; set; }       // Null si el vendedor no ha contestado
        public string CommentBySeller { get; set; }
        public DateTime? DateBySeller { get; set; }
    }

    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }

}
