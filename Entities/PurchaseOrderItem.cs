using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class PurchaseOrderItem : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }
    public Guid LastNameId { get; set; }
    public Guid LastSizeId { get; set; }
    public int QuantityRequested { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual LastName LastName { get; set; } = null!;
    public virtual LastSize LastSize { get; set; } = null!;
}