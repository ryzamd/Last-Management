using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class PurchaseOrderItemDto
{
    public Guid? Id { get; set; }
    public Guid? PurchaseOrderId { get; set; }

    [Required]
    public Guid LastNameId { get; set; }

    [Required]
    public Guid LastSizeId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QuantityRequested { get; set; }

    public string? LastCode { get; set; }
    public string? SizeLabel { get; set; }
}