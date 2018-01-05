namespace AuctionHub.Services.Contracts
{
    using Data.Models;

    public interface IBidService
    {
        Bid GetBidById(int? id);
    }
}
