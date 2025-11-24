using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class PurchaseOrder : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public Guid LocationId { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Denied
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? DenyReason { get; set; }

    public virtual Location Location { get; set; } = null!;
    public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}