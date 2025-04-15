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
        public OrderItem(int productId, string productName, decimal pricePaid, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            PricePaid = pricePaid;
            Quantity = quantity;
        }

        public OrderItem(Product product, int quantity)
            :this(product.Id, product.Name, product.Price, quantity)
        {
            Product = product;
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
