using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.LocationCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(l => l.LocationName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.LocationType)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(l => l.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(l => l.LocationCode).IsUnique();

        builder.HasMany(l => l.InventoryStocks)
            .WithOne(i => i.Location)
            .HasForeignKey(i => i.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.PurchaseOrders)
            .WithOne(p => p.Location)
            .HasForeignKey(p => p.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}