namespace AuctionHub.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

   public class AuctionConfig:IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder
                .HasOne(a => a.LastBidder)
                .WithMany(b => b.ParticipatedAuctions)
                .HasForeignKey(a => a.LastBidderId);

            builder
                .HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId);

            builder
                .HasOne(a => a.Product)
                .WithOne(p => p.Auction)
                .HasForeignKey<Auction>(a => a.ProductId);
        }
    }
}
