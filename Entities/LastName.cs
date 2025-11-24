using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class LastName : BaseEntity
{
    public string LastCode { get; set; } = string.Empty;
    public string LastType { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public string LastStatus { get; set; } = "Active";
    public Guid CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
}