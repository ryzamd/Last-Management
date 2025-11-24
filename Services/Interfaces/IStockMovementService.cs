using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services;

public interface IStockMovementService
{
    Task<PagedResult<StockMovementDto>> GetAllAsync(int page, int pageSize, string? movementType, Guid? locationId);
    Task<StockMovementDto?> GetByIdAsync(Guid id);
}