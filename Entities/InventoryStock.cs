using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class InventoryStock : BaseEntity
{
    public Guid LastNameId { get; set; }
    public Guid LastSizeId { get; set; }
    public Guid LocationId { get; set; }
    public int QuantityGood { get; set; }
    public int QuantityDamaged { get; set; }
    public int QuantityReserved { get; set; }

    public virtual LastName LastName { get; set; } = null!;
    public virtual LastSize LastSize { get; set; } = null!;
    public virtual Location Location { get; set; } = null!;
}