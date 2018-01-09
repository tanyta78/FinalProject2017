namespace AuctionHub.Services.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IBidService
    {
        Task CreateAsync(DateTime bidTime, decimal value, string userId, int auctionId);
    }
}
