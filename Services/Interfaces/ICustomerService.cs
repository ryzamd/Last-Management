using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerDto>> GetAllAsync(int page, int pageSize, string? status);
        Task<CustomerDto?> GetByIdAsync(Guid id);
        Task<CustomerDto> CreateAsync(CustomerDto dto);
        Task<CustomerDto?> UpdateAsync(Guid id, CustomerDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
