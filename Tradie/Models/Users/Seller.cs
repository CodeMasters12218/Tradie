using Tradie.Models.Enums;
using Tradie.Models.Products;

namespace Tradie.Models.Users
{
    public class Seller : User
    {
        public Seller()
        {
            Role = UserRole.Seller;
            Products = new List<Product>();
        }
        public List<Product> Products { get; private set; }

        public void PublishProduct(Product product)
        {
            Products.Add(product);
        }

        public void UpdateProductDetails(int productId, string description, decimal price)
        {
            var product = Products.FirstOrDefault(p => p.Id == productId);
        }

        public void RemoveProduct(int productId)
        {
            var product = Products.FirstOrDefault(p => p.Id == productId);
            Products.Remove(product);
        }

        public void RespondToReview(int reviewId, string response)
        {
        }

        public SalesPerformance GetSalesPerformance() { /* ... */ }

    }
}
