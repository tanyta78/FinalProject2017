using AuctionHub.Data.Models;

namespace AuctionHub.Services.Contracts
{
    public interface IBidService
    {
        Bid GetBidById(int? id);
    }
}
