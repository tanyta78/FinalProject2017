namespace AuctionHub.Services.Implementations
{
    using AuctionHub.Data;
    using AuctionHub.Data.Models;
    using AuctionHub.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductService : IProductService
    {
        private readonly AuctionHubDbContext db;

        public ProductService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public void Create(Product product, string userName)
        {
            var ownerId = this.db
                    .Users
                    .First(u => u.UserName == userName)
                    .Id;

            product.OwnerId = ownerId;

            db.Products.Add(product);
            db.SaveChanges();
        }

        public Product Details(int? productId)
        {
            var currentProduct = this.db
                .Products
                .Include(p => p.Owner.Name)
                .FirstOrDefault(p => p.Id == productId);

            return currentProduct;
        }

        public IEnumerable<Product> List() => this.db
                                    .Products
                                    .Include(p => p.Owner)
                                    .OrderByDescending(p => p.Id);
    }
}
