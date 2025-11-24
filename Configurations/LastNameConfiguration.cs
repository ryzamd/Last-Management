using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastManagement.Configurations;

public class LastNameConfiguration : IEntityTypeConfiguration<LastName>
{
    public void Configure(EntityTypeBuilder<LastName> builder)
    {
        builder.ToTable("last_names");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.LastCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.LastType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.Article)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.LastStatus)
            .HasMaxLength(20)
            .HasDefaultValue("Active");

        builder.HasIndex(l => l.LastCode).IsUnique();

        builder.HasOne(l => l.Customer)
            .WithMany(c => c.LastNames)
            .HasForeignKey(l => l.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.InventoryStocks)
            .WithOne(i => i.LastName)
            .HasForeignKey(i => i.LastNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.PurchaseOrderItems)
            .WithOne(p => p.LastName)
            .HasForeignKey(p => p.LastNameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}