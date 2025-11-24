using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class LocationDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(20)]
    public string LocationCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LocationName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string LocationType { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
}