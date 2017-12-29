namespace AuctionHub.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Auction
    {
        public int Id { get; set; }

        public DateTime Duration { get; set; }

        public decimal Price { get; set; }

        public DateTime Create { get; set; }

        public DateTime Open { get; set; }

        public DateTime Close { get; set; }

        public string LastBidderId { get; set; }

        public User LastBidder { get; set; }

        public List<Bid> Bids { get; set; } = new List<Bid>();

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
