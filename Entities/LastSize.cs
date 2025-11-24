using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class LastSize : BaseEntity
{
    public decimal SizeValue { get; set; }
    public string SizeLabel { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public Guid? ReplacementSizeId { get; set; }

    public virtual LastSize? ReplacementSize { get; set; }
    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}