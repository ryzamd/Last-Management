using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class LastNameDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(50)]
    public string LastCode { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Article { get; set; } = string.Empty;

    [StringLength(20)]
    public string LastStatus { get; set; } = "Active";

    [Required]
    public Guid CustomerId { get; set; }

    public string? CustomerName { get; set; }
    public DateTime? CreatedAt { get; set; }
}