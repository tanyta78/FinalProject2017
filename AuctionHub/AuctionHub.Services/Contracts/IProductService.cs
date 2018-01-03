namespace AuctionHub.Services.Contracts
{
    using AuctionHub.Data.Models;
    using System.Collections.Generic;

    public interface IProductService
    {
        Product Details(int? id);

        void Create(Product product, string userName);

        IEnumerable<Product> List();

        Product Delete(int? id);
    }
}
