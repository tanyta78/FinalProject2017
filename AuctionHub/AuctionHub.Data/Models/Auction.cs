namespace AuctionHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Auction
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.AuctionNameMinLength)]
        [MaxLength(DataConstants.AuctionNameMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "5000")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate{ get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string LastBidderId { get; set; }

        public User LastBidder { get; set; }

        public List<Bid> Bids { get; set; } = new List<Bid>();

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public bool IsActive { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
