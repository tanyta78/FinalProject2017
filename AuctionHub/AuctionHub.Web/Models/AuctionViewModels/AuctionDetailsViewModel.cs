using AuctionHub.Common.Mapping;
using AuctionHub.Data.Models;
using AuctionHub.Services.Models.Auctions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionHub.Web.Models.AuctionViewModels
{
    public class AuctionDetailsViewModel : IMapFrom<Auction>
    {
        public AuctionDetailsServiceModel Auction { get; set; }
        [Display(Name = "Last 10 bids")]
        public IEnumerable<Bid> LastBids { get; set; }
    }
}
