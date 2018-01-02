namespace AuctionHub.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ProductConfig:IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasOne(p => p.Owner)
                .WithMany(o => o.OwnedProducts)
                .HasForeignKey(p => p.OwnerId);

            builder.HasOne(p => p.Auction)
                   .WithOne(a => a.Product)
                   .HasForeignKey<Auction>(a => a.ProductId);
        }
    }
}
