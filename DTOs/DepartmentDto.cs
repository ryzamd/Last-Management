using System.ComponentModel.DataAnnotations;

namespace LastManagement.DTOs;

public class DepartmentDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
}