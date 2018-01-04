namespace AuctionHub.Web.Models.Product
{
    using Data;
    using Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductFormModel
    {
        [Required]
        [MinLength(DataConstants.ProductNameMinLength)]
        [MaxLength(DataConstants.ProductNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(DataConstants.ProductDescriptionMinLength)]
        [MaxLength(DataConstants.ProductDescriptionMaxLength)]
        public string Description { get; set; }
       
        public List<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
