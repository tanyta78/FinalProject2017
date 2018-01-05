namespace AuctionHub.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Data.Models;

    public class AuctionService : IAuctionService
    {
        public bool IsAuctionExist(int id)
        {
            throw new NotImplementedException();
        }

        public Auction GetAuctionById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(int id, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Auction> IndexAuctionsList()
        {
            throw new NotImplementedException();
        }
    }
}
