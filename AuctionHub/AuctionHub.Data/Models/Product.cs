namespace AuctionHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [MinLength(ProductDescriptionMinLength)]
        [MaxLength(ProductDescriptionMaxLength)]
        public string Description { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }
    }
}
