using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class StockMovementService : IStockMovementService
{
    private readonly AppDbContext _context;

    public StockMovementService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<StockMovementDto>> GetAllAsync(int page, int pageSize, string? movementType, Guid? locationId)
    {
        var query = _context.StockMovementsRepository
            .Include(m => m.LastName)
            .Include(m => m.LastSize)
            .Include(m => m.FromLocation)
            .Include(m => m.ToLocation)
            .AsQueryable();

        if (!string.IsNullOrEmpty(movementType))
            query = query.Where(m => m.MovementType == movementType);

        if (locationId.HasValue)
            query = query.Where(m => m.FromLocationId == locationId || m.ToLocationId == locationId);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new StockMovementDto
            {
                Id = m.Id,
                LastNameId = m.LastNameId,
                LastSizeId = m.LastSizeId,
                FromLocationId = m.FromLocationId,
                ToLocationId = m.ToLocationId,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                Reason = m.Reason,
                ReferenceNumber = m.ReferenceNumber,
                CreatedBy = m.CreatedBy,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<StockMovementDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<StockMovementDto?> GetByIdAsync(Guid id)
    {
        var movement = await _context.StockMovementsRepository
            .Include(m => m.LastName)
            .Include(m => m.LastSize)
            .Include(m => m.FromLocation)
            .Include(m => m.ToLocation)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movement == null)
            return null;

        return new StockMovementDto
        {
            Id = movement.Id,
            LastNameId = movement.LastNameId,
            LastSizeId = movement.LastSizeId,
            FromLocationId = movement.FromLocationId,
            ToLocationId = movement.ToLocationId,
            MovementType = movement.MovementType,
            Quantity = movement.Quantity,
            Reason = movement.Reason,
            ReferenceNumber = movement.ReferenceNumber,
            CreatedBy = movement.CreatedBy,
            CreatedAt = movement.CreatedAt
        };
    }
}