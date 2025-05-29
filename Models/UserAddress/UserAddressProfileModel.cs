using Tradie.Models.UserAddressModel;

namespace Tradie.Models.UserAddress
{
    public class UserAddressProfileModel
    {
        public IEnumerable<UsersAddressModel> Addresses { get; set; }
        public UsersAddressModel CurrentAddress { get; set; }
    }
}
