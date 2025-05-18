using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tradie.Models.Products;

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
        public string Name { get; set; }
		public string? ProfilePhotoUrl { get; set; }
		/*[Required(ErrorMessage = "Los apellidos es obligatorio.")]
        public string LastNames { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        public int Age { get; set; }*/
		[Required]
        public UserRole Role { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //[Required(ErrorMessage = "La dirección es obligatoria.")]
        /*public string Address { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }*/
        public List<Product> Products { get; set; } = new List<Product>();

    }

}
