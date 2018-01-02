namespace AuctionHub.Services.Admin.Implementations
{
    using Data;
    using Services.Admin.Models.Users;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AdminUserService : IAdminUserService
    {
        private readonly AuctionHubDbContext db;

        public AdminUserService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<UserListingServiceModel>> AllAsync()
            => await this.db
                .Users
                .ProjectTo<UserListingServiceModel>()
                .ToListAsync();
    }
}
