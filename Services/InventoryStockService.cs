using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using LastManagement.Entities;
using LastManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class InventoryStockService : IInventoryStockService
{
    private readonly AppDbContext _context;

    public InventoryStockService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<InventoryStockDto>> GetAllAsync(int page, int pageSize, Guid? locationId)
    {
        var query = _context.InventoryStocksRepository
            .Include(i => i.LastName)
            .Include(i => i.LastSize)
            .Include(i => i.Location)
            .AsQueryable();

        if (locationId.HasValue)
            query = query.Where(i => i.LocationId == locationId.Value);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InventoryStockDto
            {
                Id = i.Id,
                LastNameId = i.LastNameId,
                LastSizeId = i.LastSizeId,
                LocationId = i.LocationId,
                QuantityGood = i.QuantityGood,
                QuantityDamaged = i.QuantityDamaged,
                QuantityReserved = i.QuantityReserved,
                LastCode = i.LastName.LastCode,
                SizeLabel = i.LastSize.SizeLabel,
                LocationName = i.Location.LocationName,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<InventoryStockDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<InventoryStockDto?> GetByIdAsync(Guid id)
    {
        var stock = await _context.InventoryStocksRepository
            .Include(i => i.LastName)
            .Include(i => i.LastSize)
            .Include(i => i.Location)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (stock == null)
            return null;

        return new InventoryStockDto
        {
            Id = stock.Id,
            LastNameId = stock.LastNameId,
            LastSizeId = stock.LastSizeId,
            LocationId = stock.LocationId,
            QuantityGood = stock.QuantityGood,
            QuantityDamaged = stock.QuantityDamaged,
            QuantityReserved = stock.QuantityReserved,
            LastCode = stock.LastName.LastCode,
            SizeLabel = stock.LastSize.SizeLabel,
            LocationName = stock.Location.LocationName,
            CreatedAt = stock.CreatedAt
        };
    }

    public async Task<InventoryStockDto> CreateOrUpdateAsync(InventoryStockDto dto)
    {
        var existing = await _context.InventoryStocksRepository
            .FirstOrDefaultAsync(i => i.LastNameId == dto.LastNameId
                && i.LastSizeId == dto.LastSizeId
                && i.LocationId == dto.LocationId);

        if (existing != null)
        {
            existing.QuantityGood = dto.QuantityGood;
            existing.QuantityDamaged = dto.QuantityDamaged;
            existing.QuantityReserved = dto.QuantityReserved;
            await _context.SaveChangesAsync();

            dto.Id = existing.Id;
            dto.CreatedAt = existing.CreatedAt;
            return dto;
        }

        var stock = new InventoryStock
        {
            Id = Guid.NewGuid(),
            LastNameId = dto.LastNameId,
            LastSizeId = dto.LastSizeId,
            LocationId = dto.LocationId,
            QuantityGood = dto.QuantityGood,
            QuantityDamaged = dto.QuantityDamaged,
            QuantityReserved = dto.QuantityReserved
        };

        _context.InventoryStocksRepository.Add(stock);
        await _context.SaveChangesAsync();

        dto.Id = stock.Id;
        dto.CreatedAt = stock.CreatedAt;
        return dto;
    }

    public async Task<bool> AdjustStockAsync(Guid stockId, int quantityChange, string movementType, string reason, string createdBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var stock = await _context.InventoryStocksRepository.FindAsync(stockId);
            if (stock == null)
                return false;

            stock.QuantityGood += quantityChange;

            var movement = new StockMovement
            {
                Id = Guid.NewGuid(),
                LastNameId = stock.LastNameId,
                LastSizeId = stock.LastSizeId,
                ToLocationId = stock.LocationId,
                MovementType = movementType,
                Quantity = Math.Abs(quantityChange),
                Reason = reason,
                CreatedBy = createdBy
            };

            _context.StockMovementsRepository.Add(movement);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}