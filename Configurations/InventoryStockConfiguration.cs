using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class InventoryStockConfiguration : IEntityTypeConfiguration<InventoryStock>
{
    public void Configure(EntityTypeBuilder<InventoryStock> builder)
    {
        builder.ToTable("inventory_stocks");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.QuantityGood).HasDefaultValue(0);
        builder.Property(i => i.QuantityDamaged).HasDefaultValue(0);
        builder.Property(i => i.QuantityReserved).HasDefaultValue(0);

        builder.HasIndex(i => new { i.LastNameId, i.LastSizeId, i.LocationId }).IsUnique();

        builder.HasOne(i => i.LastName)
            .WithMany(l => l.InventoryStocks)
            .HasForeignKey(i => i.LastNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.LastSize)
            .WithMany(s => s.InventoryStocks)
            .HasForeignKey(i => i.LastSizeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Location)
            .WithMany(l => l.InventoryStocks)
            .HasForeignKey(i => i.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}