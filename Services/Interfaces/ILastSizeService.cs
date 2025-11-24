using LastManagement.DTOs;

namespace LastManagement.Services.Interfaces
{
    public interface ILastSizeService
    {
        Task<List<LastSizeDto>> GetAllAsync();
        Task<LastSizeDto?> GetByIdAsync(Guid id);
        Task<LastSizeDto> CreateAsync(LastSizeDto dto);
        Task<LastSizeDto?> UpdateAsync(Guid id, LastSizeDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
