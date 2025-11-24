using LastManagement.Entities.Base;

namespace LastManagement.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "Guest"; // Admin, Guest
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
}