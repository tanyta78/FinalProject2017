namespace AuctionHub.Services.Models.Comments
{
    using System;

    public class CommentServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
