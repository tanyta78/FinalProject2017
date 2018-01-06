namespace AuctionHub.Data.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Auction> Auctions { get; set; } = new List<Auction>();
    }
}
