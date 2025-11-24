using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("purchase_orders");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.RequestedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Department)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Pending");

        builder.Property(p => p.ReviewedBy).HasMaxLength(100);
        builder.Property(p => p.DenyReason).HasMaxLength(500);

        builder.HasIndex(p => p.OrderNumber).IsUnique();
        builder.HasIndex(p => p.Status);

        builder.HasOne(p => p.Location)
            .WithMany(l => l.PurchaseOrders)
            .HasForeignKey(p => p.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Items)
            .WithOne(i => i.PurchaseOrder)
            .HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}