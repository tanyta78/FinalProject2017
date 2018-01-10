namespace AuctionHub.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task AddAsync(string comment, string authorId, int auctionId);
    }
}
