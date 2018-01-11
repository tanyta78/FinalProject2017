namespace AuctionHub.Services.Contracts
{
    using System;
    using System.Threading.Tasks;
    using AuctionHub.Data.Models;

    public interface ICommentService
    {
        Task<int> AddAsync(string comment, string authorId, int auctionId, DateTime publishDate);

        Task DeleteAsync(int id);
        Comment GetById(int id);
        void Edit(int id, string newContent);
    }
}
