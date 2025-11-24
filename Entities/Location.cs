using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class Location : BaseEntity
{
    public string LocationCode { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty; // Production, Development, Quality, Storage
    public bool IsActive { get; set; } = true;

    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}