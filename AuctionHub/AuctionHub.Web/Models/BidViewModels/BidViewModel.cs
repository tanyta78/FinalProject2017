namespace AuctionHub.Web.Models.BidViewModels
{
    using System;

    public class BidViewModel
    {
        public int Id { get; set; }

        public DateTime BidTime { get; set; }

        public decimal Value { get; set; }
    }
}