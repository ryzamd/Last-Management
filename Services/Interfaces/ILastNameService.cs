using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services;

public interface ILastNameService
{
    Task<PagedResult<LastNameDto>> GetAllAsync(int page, int pageSize, Guid? customerId, string? status);
    Task<LastNameDto?> GetByIdAsync(Guid id);
    Task<LastNameDto> CreateAsync(LastNameDto dto);
    Task<LastNameDto?> UpdateAsync(Guid id, LastNameDto dto);
    Task<bool> DeleteAsync(Guid id);
}