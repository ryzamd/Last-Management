using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services.Interfaces
{
    public interface IInventoryStockService
    {
        Task<PagedResult<InventoryStockDto>> GetAllAsync(int page, int pageSize, Guid? locationId);
        Task<InventoryStockDto?> GetByIdAsync(Guid id);
        Task<InventoryStockDto> CreateOrUpdateAsync(InventoryStockDto dto);
        Task<bool> AdjustStockAsync(Guid stockId, int quantityChange, string movementType, string reason, string createdBy);
    }
}
