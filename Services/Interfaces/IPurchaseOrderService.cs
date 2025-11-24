using LastManagement.DTOs;
using LastManagement.DTOs.Shared;

namespace LastManagement.Services;

public interface IPurchaseOrderService
{
    Task<PagedResult<PurchaseOrderDto>> GetAllAsync(int page, int pageSize, string? status);
    Task<PurchaseOrderDto?> GetByIdAsync(Guid id);
    Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto dto, string requestedBy);
    Task<bool> ConfirmOrderAsync(Guid id, string reviewedBy);
    Task<bool> DenyOrderAsync(Guid id, string reviewedBy, string reason);
}