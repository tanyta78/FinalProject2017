namespace AuctionHub.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;

    public class BidService :  IBidService
    {
        private readonly AuctionHubDbContext db;

        public BidService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(DateTime bidTime, decimal value, string userId, int auctionId)
        {
            var bid = new Bid
            {
                BidTime = bidTime,
                Value = value,
                UserId = userId,
                AuctionId = auctionId
            };

            await this.db.Bids.AddAsync(bid);
            await this.db.SaveChangesAsync();
        }
    }
}
