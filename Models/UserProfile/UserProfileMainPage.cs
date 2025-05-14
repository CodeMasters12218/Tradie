using Tradie.Models.UserProfile;

namespace Tradie.Models.UserProfile
{
	public class UserProfileMainPageModel
	{
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public List<string>? Orders { get; set; }
		// Add other properties like WishList, AccountStatus, etc.
	}
}
