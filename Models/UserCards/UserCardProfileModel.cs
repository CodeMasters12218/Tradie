using System.ComponentModel.DataAnnotations.Schema;

namespace Tradie.Models.UserCards
{
    public class UserCardProfileModel
    {
        [NotMapped]
        public IEnumerable<UserCardModel> Cards { get; set; }
        public UserCardModel CurrentCard { get; set; }
    }
}

