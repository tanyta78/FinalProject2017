namespace AuctionHub.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class AuctionService : IAuctionService
    {
        private AuctionHubDbContext db;

        public AuctionService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public bool IsAuctionExist(int id)
        {
            return this.db.Auctions.Any(a => a.Id == id);
        }

        public Auction GetAuctionById(int id)
        {
           return this.db.Auctions.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Auction> GetByCategory(string category)
        {
            var auctions = this.db.Categories.FirstOrDefault(c => c.Name == category).Auctions;
            return auctions;
        }

        public void Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId)
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

            this.db.SaveChanges();

            //make connection with category and product
            var category = this.db.Categories.FindAsync(categoryId).Result;
            var product = this.db.Products.FindAsync(productId).Result;
            category.Auctions.Add(auction);
            product.Auction = auction;
            this.db.SaveChanges();

        }

        public void Delete(int id)
        {
            var auction = this.GetAuctionById(id);

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

            this.db.SaveChanges();
        }

        public void Edit(int id, DateTime endDate)
        {
            var auction = this.GetAuctionById(id);
            if (endDate< DateTime.Now)
            {
                return;
            }
            auction.EndDate = endDate;
            this.db.SaveChanges();
        }

        public IEnumerable<Auction> IndexAuctionsList()
        {
            IEnumerable<Auction> auctionsToView = this.db.Auctions
                                                      .Include(a => a.Product)
                                                      .Take(DataConstants.AuctionToShow);
            return auctionsToView;
        }

       
    }
}
