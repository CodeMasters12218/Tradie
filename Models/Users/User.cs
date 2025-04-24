using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.Users
{
    public enum UserRole
    {
        Admin,
        Seller,
        Customer
    }

    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
