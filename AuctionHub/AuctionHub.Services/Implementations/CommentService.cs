namespace AuctionHub.Services.Implementations
{
    using Contracts;
    using Data;
    using Data.Models;
    using System;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly AuctionHubDbContext db;

        public CommentService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(string comment, string authorId, int auctionId)
        {
            var result = new Comment
            {
                Content = comment,
                PublishDate = DateTime.UtcNow,
                AuthorId = authorId,
                AuctionId = auctionId
            };

            this.db.Add(result);

            await this.db.SaveChangesAsync();
        }
    }
}
