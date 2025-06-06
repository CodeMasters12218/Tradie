using Tradie.Models.Users;

namespace Tradie.Models.Products
{
    public class ReviewAboutYouViewModel
    {
        public string OrderNumber { get; set; }
        public ProductSummaryDto Product { get; set; }
        public int Quantity { get; set; }

        // Datos del cliente que te valoró
        public string ReviewerName { get; set; }   // Nombre del cliente
        public int ReviewerRating { get; set; }
        public string ReviewerComment { get; set; }
        public DateTime ReviewerDate { get; set; }

        // Datos de tu respuesta (si ya has respondido)
        public int? SellerRating { get; set; }
        public string SellerComment { get; set; }
        public DateTime? SellerDate { get; set; }
    }

}
