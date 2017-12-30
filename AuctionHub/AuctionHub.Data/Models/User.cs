namespace AuctionHub.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(UserNameMinLength)]
        [MaxLength(UserNameMaxLength)]
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public Gender Gender { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public List<Product> OwnedProducts { get; set; } = new List<Product>();

        public List<Bid> Bids { get; set; } = new List<Bid>();

        public List<Auction> ParticipatedAuctions { get; set; } = new List<Auction>();

        public List<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
