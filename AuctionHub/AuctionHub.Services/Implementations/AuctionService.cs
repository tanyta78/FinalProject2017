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
            throw new NotImplementedException();
        }

        public IEnumerable<Auction> GetByCategory(string category)
        {
            throw new NotImplementedException();
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
               ProductId = productId
           };

            this.db.Auctions.Add(auction);

            this.db.SaveChanges();

            //make connection with category and product

        }

        public void Delete(int id)
        {
            var auction = this.db.Auctions.FirstOrDefault(a => a.Id == id);

            this.db.Auctions.Remove(auction);

            this.db.SaveChanges();
        }

        public void Edit(int id, DateTime endDate)
        {
            throw new NotImplementedException();
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
