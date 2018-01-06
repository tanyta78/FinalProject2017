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
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly AuctionHubDbContext db;

        public ProductService(AuctionHubDbContext db)
        {
            this.db = db;
        }
        
        public async Task CreateAsync(string name, string description, List<Picture> pictures, string ownerId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Pictures = pictures,
                OwnerId = ownerId
            };

            this.db.Products.Add(product);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name, string description)
        {
            var productToBeEdited = await this.db
                .Products
                .FindAsync(id);

            if (productToBeEdited == null)
            {
                return;
            }

            productToBeEdited.Name = name;
            productToBeEdited.Description = description;

            this.db.Entry(productToBeEdited).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var productToBeDeleted = await this.db.Products.FindAsync(id);
            if (productToBeDeleted == null)
            {
                return;
            }

            // Here, before we delete the product, its pictures in the file system should be deleted as well!

            this.db.Products.Remove(productToBeDeleted);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductListingServiceModel>> ListAsync(string ownerId) 
            => await this.db
                  .Products
                  .Where(p => p.OwnerId == ownerId)
                  .ProjectTo<ProductListingServiceModel>()
                  .OrderByDescending(p => p.Id)
                  .ToListAsync();
        
        public Task<ProductDetailsServiceModel> GetProductByIdAsync(int id)
            => this.db
                .Products
                .Where(p => p.Id == id)
                .ProjectTo<ProductDetailsServiceModel>()
                .FirstOrDefaultAsync();

        public bool IsProductExist(int id)
        {
            return this.db.Products.Any(p => p.Id == id);
        }

        public int GetProductPicturesCount(int id)
        {
            var count = this.db
                .Pictures
                .Count(p => p.ProductId == id);

            return count;
        }

        public List<Picture> GetProductPictures(int id)
        {
            var allPictures = this.db
                .Pictures
                .Where(p => p.ProductId == id)
                .ToList();

            return allPictures;
        }
    }
}
