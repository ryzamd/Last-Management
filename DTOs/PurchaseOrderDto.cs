using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class PurchaseOrderDto
{
    public Guid? Id { get; set; }
    public string? OrderNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string RequestedBy { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    public Guid LocationId { get; set; }

    public string Status { get; set; } = "Pending";
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? DenyReason { get; set; }

    public string? LocationName { get; set; }
    public List<PurchaseOrderItemDto> Items { get; set; } = new();
    public DateTime? CreatedAt { get; set; }
}