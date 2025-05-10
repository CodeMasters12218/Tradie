namespace Tradie.Models.Users
{
    public class UserManagementViewModel
    {
        public IEnumerable<AdminUserViewModel> Users { get; set; }
        public AdminUserViewModel CurrentUser { get; set; }
    }
}
