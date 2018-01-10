namespace AuctionHub.Web.Models.AuctionViewModels
{
    using AuctionHub.Services.Models.Auctions;
    using Common.Mapping;
    using Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AuctionDetailsViewModel : IMapFrom<Auction>
    {
        public AuctionDetailsServiceModel Auction { get; set; }
        [Display(Name = "Last 10 bids")]
        
        public IEnumerable<Bid> LastBids { get; set; }
        public decimal CurrentBid { get; set; }
    }
}
