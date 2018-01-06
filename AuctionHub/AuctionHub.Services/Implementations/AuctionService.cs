namespace AuctionHub.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models.Auctions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuctionService : IAuctionService
    {
        private readonly AuctionHubDbContext db;

        public AuctionService(AuctionHubDbContext db)
        {
            this.db = db;
        }
        
        public async Task<AuctionDetailsServiceModel> GetAuctionByIdAsync(int id)
            => await this.db
                 .Auctions
                 .Where(a => a.Id == id)
                 .ProjectTo<AuctionDetailsServiceModel>()
                 .FirstOrDefaultAsync();

        public async Task<IEnumerable<AuctionDetailsServiceModel>> GetByCategoryNameAsync(string categoryName)
             => await this.db
                .Auctions
                .Where(a => a.Category.Name == categoryName)
                .ProjectTo<AuctionDetailsServiceModel>()
                .ToListAsync();

        public async Task Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId)
        {
           var auction = new Auction()
           {
               Description = description,
               Price = price,
               StartDate = startDate,
               EndDate = endDate,
               CategoryId = categoryId,
               ProductId = productId,
             //  IsActive = true
           };

            this.db.Auctions.Add(auction);

            await this.db.SaveChangesAsync();

            //make connection with category and product
            var category = this.db.Categories.FindAsync(categoryId).Result;
            var product = this.db.Products.FindAsync(productId).Result;
            category.Auctions.Add(auction);
            product.Auction = auction;
            await this.db.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var auction = await this.db.Auctions.FindAsync(id);

            //delete connection with product
            var auctionProductId = auction.ProductId;
            this.db.Products.FindAsync(auctionProductId).Result.AuctionId = null;
            
            //delete connection with category
            var auctionCategoryId = auction.CategoryId;
            this.db.Categories.FindAsync(auctionCategoryId).Result.Auctions.Remove(auction);


            // delete connection with bidding??? NOT FINNISHED

           var bidsList= this.db.Bids.Where(b => b.AuctionId == id);
            
            this.db.SaveChanges();
            
            // auction.IsActive = false;
            this.db.Auctions.Remove(auction);

            await this.db.SaveChangesAsync();
        }

        public async Task Edit(int id, DateTime endDate)
        {
            var auction = await this.GetAuctionByIdAsync(id);

            if (endDate < DateTime.UtcNow)
            {
                return;
            }
            auction.EndDate = endDate;

            await this.db.SaveChangesAsync();
        }

        public IEnumerable<Auction> IndexAuctionsList()
        {
            var auctionsToView = this.db.Auctions
                                                      .Include(a => a.Product)
                                                      .Take(DataConstants.AuctionToShow);
                                                      //.ToListAsync();
            return auctionsToView;
        }

        public bool IsAuctionExist(int id)
        {
            return this.db.Auctions.Any(a => a.Id == id);
        }
    }
}
