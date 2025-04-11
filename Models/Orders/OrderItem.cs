using Tradie.Models.Products;

namespace Tradie.Models.Orders
{

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Canceled,
        Refunded
    }
    public class OrderItem
    {
        public OrderItem(Product product, int productId, string productName, decimal pricePaid, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            ProductId = product.Id;
            ProductName = product.Name;
            PricePaid = product.Price;
            Quantity = quantity;
        }

        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ProductName { get; set; } 
        public decimal PricePaid { get; set; }
        public int Quantity { get; set; }
    }
}
