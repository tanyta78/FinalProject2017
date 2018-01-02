namespace AuctionHub.Web.Models.AuctionViewModels
{
    using System;
    using System.Collections.Generic;
    using Data.Models;

    public class IndexAuctionViewModel
    {
        public int Id { get; set; }

        public DateTime Duration { get; set; }

        public decimal LastBiddedPrice { get; set; }

        public string OwnerName { get; set; }

        public string ProductName { get; set; }

        public string PicturePath { get; set; }
    }
}
