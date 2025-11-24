using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("stock_movements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.MovementType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Reason).HasMaxLength(500);
        builder.Property(m => m.ReferenceNumber).HasMaxLength(50);
        builder.Property(m => m.CreatedBy).HasMaxLength(100);

        builder.HasIndex(m => m.CreatedAt);
        builder.HasIndex(m => m.MovementType);

        builder.HasOne(m => m.LastName)
            .WithMany()
            .HasForeignKey(m => m.LastNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.LastSize)
            .WithMany(s => s.StockMovements)
            .HasForeignKey(m => m.LastSizeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.FromLocation)
            .WithMany()
            .HasForeignKey(m => m.FromLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(m => m.ToLocation)
            .WithMany()
            .HasForeignKey(m => m.ToLocationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}