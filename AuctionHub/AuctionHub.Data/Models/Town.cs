namespace AuctionHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Town
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TownNameMinLength)]
        [MaxLength(TownNameMaxLength)]
        public string Name { get; set; }

        public List<Address> Addresses { get; set; } = new List<Address>();
    }
}
