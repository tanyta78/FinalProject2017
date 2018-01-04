namespace AuctionHub.Services.Models.Products
{
    using Common.Mapping;
    using Data.Models;

    public class ProductListingServiceModel : IMapFrom<Product>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string OwnerId { get; set; }
    }
}
