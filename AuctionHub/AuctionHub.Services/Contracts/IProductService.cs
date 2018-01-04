namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Services.Models.Products;
    using System.Collections.Generic;

    public interface IProductService
    {
        void Create(string name, string description, List<Picture> pictures, string ownerId);

        IEnumerable<ProductListingServiceModel> List();

        Product GetProductById(int? id);
    }
}
