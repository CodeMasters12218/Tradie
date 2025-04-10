using Tradie.Models.Users;

namespace Tradie.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }

        public Order(int id, Customer customer, DateTime orderDate, OrderStatus status, List<OrderItem> items)
        {
            Id = id;
            Customer = customer;
            OrderDate = orderDate;
            Status = status;
            Items = items ?? new List<OrderItem>(); // Evita valores nulos
        }
    }
}
