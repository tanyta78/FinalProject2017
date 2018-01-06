namespace AuctionHub.Services.Models.Auctions
{
    using AuctionHub.Common.Mapping;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AuctionDetailsServiceModel : IMapFrom<Auction>
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string LastBidderId { get; set; }

        public List<Bid> Bids { get; set; } = new List<Bid>();

        public int CategoryId { get; set; }

        public int ProductId { get; set; }

        public bool IsActive { get; set; }
    }
}
