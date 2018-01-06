namespace AuctionHub.Services.Models.Products
{
    using Common.Mapping;
    using Data.Models;
    using System.Collections.Generic;

    public class ProductDetailsServiceModel : IMapFrom<Product>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string OwnerId { get; set; }

        public List<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
