namespace AuctionHub.Services.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task AddAsync(string comment, string authorId, int auctionId, DateTime publishDate);

        Task DeleteAsync(int id);
    }
}
