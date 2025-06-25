namespace Tradie.Models.Products
{
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Category? Category { get; set; }        
        public string? Subcategory { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }

}
