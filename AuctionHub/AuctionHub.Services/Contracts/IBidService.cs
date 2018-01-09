namespace AuctionHub.Services.Contracts
{
    using AuctionHub.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBidService
    {
        Task CreateAsync(DateTime bidTime, decimal value, string userId, int auctionId);
        IEnumerable<Bid> GetForAuction(int auctionId);
    }
}
