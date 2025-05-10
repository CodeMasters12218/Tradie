using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.Users
{
    public class AdminUserViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }
    }

}
