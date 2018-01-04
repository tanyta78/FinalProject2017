namespace AuctionHub.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using Services.Models.Products;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductService : IProductService
    {
        private readonly AuctionHubDbContext db;

        public ProductService(AuctionHubDbContext db)
        {
            this.db = db;
        }


        public void Create(string name, string description, List<Picture> pictures, string ownerId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Pictures = pictures,
                OwnerId = ownerId
            };

            this.db.Add(product);

            this.db.SaveChanges();
        }
        
        public IEnumerable<ProductListingServiceModel> List() 
            => this.db
                  .Products
                  .ProjectTo<ProductListingServiceModel>()
                  .OrderByDescending(p => p.Id)
                  .ToList();
        
        public Product GetProductById(int? id)
        {
            var product = this.db
                .Products
                .Include(p => p.Owner)
                .FirstOrDefault(p => p.Id == id);

            return product;
        }
    }
}
