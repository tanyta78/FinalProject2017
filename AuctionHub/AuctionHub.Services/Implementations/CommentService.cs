namespace AuctionHub.Services.Implementations
{
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly AuctionHubDbContext db;

        public CommentService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAsync(string comment, string authorId, int auctionId, DateTime publishDate)
        {
            var result = new Comment
            {
                Content = comment,
                PublishDate = publishDate,
                AuthorId = authorId,
                AuctionId = auctionId
            };

            this.db.Add(result);

            await this.db.SaveChangesAsync();
            return result.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var currentComment = await this.db
                .Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (currentComment == null)
            {
                return;
            }

            this.db.Remove(currentComment);
            await this.db.SaveChangesAsync();
        }

        public Comment GetById(int id) => this.db.Comments.FirstOrDefault(c => c.Id == id);

        public void Edit(int id, string newContent)
        {
            Comment comment = this.db.Comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                comment.Content = newContent;
                this.db.SaveChanges();
            }
        }
    }
}
