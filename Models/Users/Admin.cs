using Microsoft.AspNetCore.Identity;
using Tradie.Models.Orders;
using Tradie.Models.Products;
using Tradie.Models.Reports;

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
        public List<User> SearchUsers(string filer) {
            return new List<User>();
        }

        public void AddProduct(Product product) {  }
        public void UpdateProduct(Product product) {  }
        public void DeleteProduct(int productId) {  }

        public List<Order> GetAllOrders() {
            return new List<Order>();
        }
        public void UpdateOrderStatus(int orderId, OrderStatus status) {  }
        public void ProcessRefund(int orderId) {  }

        public SalesReport GenerateSalesReport(DateTime start, DateTime end) {
            return new SalesReport();
        }
        public void ExportReport(Report report, ExportFormat format) {  }
    }
}
