namespace AuctionHub.Web.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using static AuctionHub.Data.DataConstants;

    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ProductNameMinLength)]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(ProductDescriptionMinLength)]
        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
