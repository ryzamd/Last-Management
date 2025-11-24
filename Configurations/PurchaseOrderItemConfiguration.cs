using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.ToTable("purchase_order_items");

        builder.HasKey(i => i.Id);

        builder.HasOne(i => i.PurchaseOrder)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.LastName)
            .WithMany(l => l.PurchaseOrderItems)
            .HasForeignKey(i => i.LastNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.LastSize)
            .WithMany()
            .HasForeignKey(i => i.LastSizeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}