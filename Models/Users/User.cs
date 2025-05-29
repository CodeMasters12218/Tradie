using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tradie.Models.Products;
using Tradie.Models.UserCards;
using Tradie.Models.Users;

public class User : IdentityUser<int>
{
	[Required(ErrorMessage = "El nombre es obligatorio.")]
	public string? Name { get; set; }
	[Required(ErrorMessage = "Los apellidos es obligatorio.")]
	public string? LastNames { get; set; }
	[Required]
	public UserRole Role { get; set; }
	public List<Product> Products { get; set; } = new List<Product>();
	public string? ProfilePhotoUrl { get; set; }
	[NotMapped]
	public UserCardProfileModel? UserCardProfile { get; set; } = new UserCardProfileModel();
}
