using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.Users
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastNames { get; set; }

    }
}
