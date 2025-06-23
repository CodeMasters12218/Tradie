using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tradie.Models.UserCards
{
    public class UserCardProfileModel
    {
        [NotMapped]
        [ValidateNever]
        public IEnumerable<UserCardModel> Cards { get; set; }
        [ValidateNever]
        public UserCardModel CurrentCard { get; set; }
    }
}

