using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class StockMovement : BaseEntity
{
    public Guid LastNameId { get; set; }
    public Guid LastSizeId { get; set; }
    public Guid? FromLocationId { get; set; }
    public Guid? ToLocationId { get; set; }
    public string MovementType { get; set; } = string.Empty; // Purchase, Transfer, Adjustment, Damage, Reserve
    public int Quantity { get; set; }
    public string? Reason { get; set; }
    public string? ReferenceNumber { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public virtual LastName LastName { get; set; } = null!;
    public virtual LastSize LastSize { get; set; } = null!;
    public virtual Location? FromLocation { get; set; }
    public virtual Location? ToLocation { get; set; }
}