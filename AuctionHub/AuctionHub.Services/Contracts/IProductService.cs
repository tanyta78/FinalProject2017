namespace AuctionHub.Services.Contracts
{
    using AuctionHub.Data.Models;
    using System.Collections.Generic;

    public interface IProductService
    {
        void Create(Product product, string userName);

        IEnumerable<Product> List();

        Product GetProductById(int? id);
    }
}
