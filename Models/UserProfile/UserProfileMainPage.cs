namespace Tradie.Models.UserProfile
{
	public class UserProfileMainPageModel
	{
		public string? ProfilePhotoUrl { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public List<string>? Orders { get; set; }
	}
}
