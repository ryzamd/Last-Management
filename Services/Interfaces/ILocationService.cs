using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services.Interfaces
{
    public interface ILocationService
    {
        Task<PagedResult<LocationDto>> GetAllAsync(int page, int pageSize, string? locationType, bool? isActive);
        Task<LocationDto?> GetByIdAsync(Guid id);
        Task<LocationDto> CreateAsync(LocationDto dto);
        Task<LocationDto?> UpdateAsync(Guid id, LocationDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
