namespace AuctionHub.Web.Models.AuctionViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data;

    public class AuctionFormModel
    {
   
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
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }

        public int ProductId { get; set; }

        public bool IsActive { get; set; }
    }
}