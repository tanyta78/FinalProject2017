namespace AuctionHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Address
    {
        public int Id { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        [Required]
        [MinLength(AddressCountryMinLength)]
        [MaxLength(AddressCountryMaxLength)]
        public string Country { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
