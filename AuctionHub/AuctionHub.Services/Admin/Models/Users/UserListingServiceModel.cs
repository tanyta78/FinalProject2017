namespace AuctionHub.Services.Admin.Models.Users
{
    using Common.Mapping;
    using Data.Models;

    public class UserListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
