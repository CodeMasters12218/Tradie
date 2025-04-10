namespace Tradie.Models.Users
{
    public enum UserRole
    {
        Admin,
        Seller,
        Customer
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
