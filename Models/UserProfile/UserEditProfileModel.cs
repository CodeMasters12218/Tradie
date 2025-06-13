using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.UserProfile
{
	public class UserEditProfileModel
	{
		[Required(ErrorMessage = "El nombre es obligatorio")]
		[Display(Name = "Nombre")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Los apellidos son obligatorios")]
		[Display(Name = "Apellidos")]
		public string? LastNames { get; set; }

		[Required(ErrorMessage = "El correo electrónico es obligatorio")]
		[EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
		[Display(Name = "Correo electrónico")]
		public string? Email { get; set; }

		[Phone(ErrorMessage = "Formato de teléfono inválido")]
		[Display(Name = "Teléfono")]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Dirección")]
		public string? Address { get; set; }

		[Display(Name = "País")]
		public string? Country { get; set; }

		[Display(Name = "Ciudad")]
		public string? City { get; set; }

		[Display(Name = "Región")]
		public string? Region { get; set; }

		[Display(Name = "Código postal")]
		public string? PostalCode { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Nueva contraseña")]
		public string? NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirmar contraseña")]
		[Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
		public string? ConfirmPassword { get; set; }

		// Keep for backward compatibility
		public string? FullName => $"{Name} {LastNames}".Trim();
	}
}