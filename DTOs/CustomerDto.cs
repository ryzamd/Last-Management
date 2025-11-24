using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class CustomerDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}