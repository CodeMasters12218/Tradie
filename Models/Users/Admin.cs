using Microsoft.AspNetCore.Identity;
using Tradie.Models.Enums;
using Tradie.Models.Orders;
using Tradie.Models.Products;

namespace Tradie.Models.Users
{
    public class Admin : User
    {
        public Admin()
        {
            Role = UserRole.Admin;
        }
        public void CreateUser(User newUser) { }
        public void UpdateUser(User user) { }
        public void DeleteUser(int userId) { }
        public List<User> SearchUsers(string filer) { }

        public void AddProduct(Product product) {  }
        public void UpdateProduct(Product product) {  }
        public void DeleteProduct(int productId) {  }

        public List<Order> GetAllOrders() {  }
        public void UpdateOrderStatus(int orderId, OrderStatus status) {  }
        public void ProcessRefund(int orderId) {  }

        public SalesReport GenerateSalesReport(DateTime start, DateTime end) {  }
        public void ExportReport(Report report, ExportFormat format) {  }
    }
}
