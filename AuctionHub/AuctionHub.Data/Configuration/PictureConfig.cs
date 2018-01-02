namespace AuctionHub.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PictureConfig : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder
                .HasOne(p => p.Author)
                .WithMany(a => a.Pictures)
                .HasForeignKey(p => p.AuthorId);

            builder
                .HasOne(p => p.Product)
                .WithMany(pr => pr.Pictures)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
