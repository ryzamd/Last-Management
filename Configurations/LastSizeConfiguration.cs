using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class LastSizeConfiguration : IEntityTypeConfiguration<LastSize>
{
    public void Configure(EntityTypeBuilder<LastSize> builder)
    {
        builder.ToTable("last_sizes");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SizeValue)
            .HasPrecision(4, 1)
            .IsRequired();

        builder.Property(s => s.SizeLabel)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Active");

        builder.HasIndex(s => s.SizeValue).IsUnique();

        builder.HasOne(s => s.ReplacementSize)
            .WithMany()
            .HasForeignKey(s => s.ReplacementSizeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.InventoryStocks)
            .WithOne(i => i.LastSize)
            .HasForeignKey(i => i.LastSizeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.StockMovements)
            .WithOne(m => m.LastSize)
            .HasForeignKey(m => m.LastSizeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}