namespace AuctionHub.Data
{
    using Configuration;
    using Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AuctionHubDbContext : IdentityDbContext<User>
    {
        public AuctionHubDbContext(DbContextOptions<AuctionHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new AuctionConfig());
            builder.ApplyConfiguration(new BidConfig());
            builder.ApplyConfiguration(new CategoryConfig());
            builder.ApplyConfiguration(new PictureConfig());

           base.OnModelCreating(builder);
        }
    }
}
