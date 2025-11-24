using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class Department : BaseEntity
{
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}