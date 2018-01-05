namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Services.Models.Products;
    using System.Collections.Generic;

    public interface IProductService
    {
        void Create(string name, string description, List<Picture> pictures, string ownerId);

        void Edit(int id, string name, string description);

        IEnumerable<ProductListingServiceModel> List();

        Product GetProductById(int? id);

        bool IsProductExist(int id);

        void Delete(int id);
    }
}
