namespace AuctionHub.Data.Models
{
    public class Picture
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}