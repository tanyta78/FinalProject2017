using System;

namespace AuctionHub.Data.Models
{
    public class Bid
    {
        public int Id { get; set; }

        public DateTime BidTime { get; set; }

        public decimal Value { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int AuctionId { get; set; }

        public Auction Auction { get; set; }
    }
}
