using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo incorrecto.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string PasswordHash { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
