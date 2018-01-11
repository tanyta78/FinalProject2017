namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Services.Models.Auctions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAuctionService
    {
        bool IsAuctionExist(int id);

        Task<AuctionDetailsServiceModel> GetAuctionByIdAsync(int id);

        Task Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId);

        Task Delete(int id);

        Task Edit(int id, string productName, string description, string categoryName, int productId);

        IEnumerable<Auction> IndexAuctionsList();

        Task<IEnumerable<AuctionDetailsServiceModel>> GetByCategoryNameAsync(string categoryName);

       Task<IQueryable<Auction>> ListAsync(string ownerId, int page, string search);
    }
}
