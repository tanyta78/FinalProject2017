namespace AuctionHub.Data
{
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Address>()
                .HasMany(a => a.Users)
                .WithOne(u => u.Address)
                .HasForeignKey(u => u.AddressId);

            builder
                .Entity<Address>()
                .HasOne(a => a.Town)
                .WithMany(t => t.Addresses)
                .HasForeignKey(a => a.TownId);

            builder
                .Entity<Auction>()
                .HasOne(a => a.LastBidder)
                .WithMany(b => b.ParticipatedAuctions)
                .HasForeignKey(a => a.LastBidderId);

            builder
                .Entity<Auction>()
                .HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId);

            builder
                .Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId);

            builder
                .Entity<Product>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.OwnedProducts)
                .HasForeignKey(p => p.OwnerId);

            base.OnModelCreating(builder);
        }
    }
}
