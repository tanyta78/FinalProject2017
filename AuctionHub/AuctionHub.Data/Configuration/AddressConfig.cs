namespace AuctionHub.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasMany(a => a.Users)
                .WithOne(u => u.Address)
                .HasForeignKey(u => u.AddressId);

            builder
                .HasOne(a => a.Town)
                .WithMany(t => t.Addresses)
                .HasForeignKey(a => a.TownId);
        }
    }
}
