using LastManagement.Constants;
using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class Customer : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public string Status { get; set; } = AppConstants.Status.Active; // Active, Inactive, Suspended

    public virtual ICollection<LastName> LastNames { get; set; } = new List<LastName>();
}