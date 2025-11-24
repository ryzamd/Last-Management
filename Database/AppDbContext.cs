using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> UsersRepository => Set<User>();
    public DbSet<Customer> CustomersRepository => Set<Customer>();
    public DbSet<Location> LocationsRepository => Set<Location>();
    public DbSet<Department> DepartmentsRepository => Set<Department>();
    public DbSet<LastSize> LastSizesRepository => Set<LastSize>();
    public DbSet<LastName> LastNamesRepository => Set<LastName>();
    public DbSet<InventoryStock> InventoryStocksRepository => Set<InventoryStock>();
    public DbSet<StockMovement> StockMovementsRepository => Set<StockMovement>();
    public DbSet<PurchaseOrder> PurchaseOrdersRepository => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItemsRepository => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is Entities.Base.BaseEntity
                                                    && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Entities.Base.BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}