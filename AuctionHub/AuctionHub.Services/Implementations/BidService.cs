using System.Linq;
using AuctionHub.Data;
using AuctionHub.Data.Models;
using AuctionHub.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Services.Implementations
{
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
