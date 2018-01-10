namespace AuctionHub.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class BidService :  IBidService
    {
        private readonly AuctionHubDbContext db;
        private readonly UserManager<User> userManager;


        public BidService(AuctionHubDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task CreateAsync(DateTime bidTime, decimal value, string userId, int auctionId)
        {
            User bidder = await this.userManager.FindByIdAsync(userId);
            var bid = new Bid
            {
                BidTime = bidTime,
                Value = value,
                UserId = bidder.Id,
                User = bidder,
                AuctionId = auctionId,
            };

            await this.db.Bids.AddAsync(bid);
            await this.db.SaveChangesAsync();
        }

        public IEnumerable<Bid> GetForAuction(int auctionId) 
            =>this.db.Bids.Include(b => b.User).Where(b => b.AuctionId == auctionId).ToList();
        
    }
}
