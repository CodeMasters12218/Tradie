using Tradie.Models.Products;
using Tradie.Models.Users;

namespace Tradie.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        private Order()
        {
            Items = new List<OrderItem>();
        }
        public Order(int id, int customerId, DateTime orderDate, OrderStatus status, List<OrderItem> items)
        {
            Id = id;
            CustomerId = customerId;
            OrderDate = orderDate;
            Status = status;
            Items = items ?? new List<OrderItem>(); // Evita valores nulos
        }
    }
}
