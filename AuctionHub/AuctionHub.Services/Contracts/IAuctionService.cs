namespace AuctionHub.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Data.Models;

    public interface IAuctionService
    {
        bool IsAuctionExist(int id);

        Auction GetAuctionById(int id);

        void Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId);

        void Delete(int id);

        void Edit(int id, DateTime endDate);

        IEnumerable<Auction> IndexAuctionsList();

        IEnumerable<Auction> GetByCategory(string category);

    }
}
