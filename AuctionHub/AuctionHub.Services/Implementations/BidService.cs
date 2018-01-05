namespace AuctionHub.Services.Implementations
{
    using System.Linq;
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

        public Bid GetBidById(int? id)
        {
            var bid = this.db
                .Bids
                .FirstOrDefault(p => p.Id == id);

            return bid;
        }
    }
}
