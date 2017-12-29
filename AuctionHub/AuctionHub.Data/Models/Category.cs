using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionHub.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Auction> Auctions { get; set; } = new List<Auction>();
    }
}
