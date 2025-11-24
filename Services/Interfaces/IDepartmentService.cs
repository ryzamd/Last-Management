using LastManagement.DTOs;

namespace LastManagement.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(Guid id);
        Task<DepartmentDto> CreateAsync(DepartmentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
