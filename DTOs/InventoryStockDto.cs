using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class InventoryStockDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid LastNameId { get; set; }

    [Required]
    public Guid LastSizeId { get; set; }

    [Required]
    public Guid LocationId { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityGood { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityDamaged { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityReserved { get; set; }

    // Response fields
    public string? LastCode { get; set; }
    public string? SizeLabel { get; set; }
    public string? LocationName { get; set; }
    public DateTime? CreatedAt { get; set; }
}