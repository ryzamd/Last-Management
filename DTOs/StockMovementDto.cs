using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class StockMovementDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid LastNameId { get; set; }

    [Required]
    public Guid LastSizeId { get; set; }

    public Guid? FromLocationId { get; set; }
    public Guid? ToLocationId { get; set; }

    [Required]
    [StringLength(20)]
    public string MovementType { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    [StringLength(50)]
    public string? ReferenceNumber { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
}