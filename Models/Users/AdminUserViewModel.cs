using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.Users
{
	public enum UserRole
	{
		Admin,
		Seller,
		Customer
	}

	public class AdminUserViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Los apellidos son obligatorios")]
		public string? LastNames { get; set; }

		[Required(ErrorMessage = "El rol es obligatorio.")]
		public UserRole Role { get; set; }

		[Required(ErrorMessage = "El correo es obligatorio")]
		[EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "La contraseña es obligatoria.")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
		public string? Password { get; set; }
		public string? ProfilePhotoUrl { get; set; }
	}
}
