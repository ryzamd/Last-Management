using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class LastSizeDto
{
    public Guid? Id { get; set; }

    [Required]
    [Range(0, 99.9)]
    public decimal SizeValue { get; set; }

    [Required]
    [StringLength(20)]
    public string SizeLabel { get; set; } = string.Empty;

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    public Guid? ReplacementSizeId { get; set; }
    public DateTime? CreatedAt { get; set; }
}