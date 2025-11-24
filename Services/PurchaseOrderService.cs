using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _context;

    public PurchaseOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PurchaseOrderDto>> GetAllAsync(int page, int pageSize, string? status)
    {
        var query = _context.PurchaseOrdersRepository.Include(p => p.Location).Include(p => p.Items).AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PurchaseOrderDto
            {
                Id = p.Id,
                OrderNumber = p.OrderNumber,
                RequestedBy = p.RequestedBy,
                Department = p.Department,
                LocationId = p.LocationId,
                Status = p.Status,
                ReviewedBy = p.ReviewedBy,
                ReviewedAt = p.ReviewedAt,
                DenyReason = p.DenyReason,
                LocationName = p.Location.LocationName,
                CreatedAt = p.CreatedAt,
                Items = p.Items.Select(i => new PurchaseOrderItemDto
                {
                    Id = i.Id,
                    LastNameId = i.LastNameId,
                    LastSizeId = i.LastSizeId,
                    QuantityRequested = i.QuantityRequested
                }).ToList()
            })
            .ToListAsync();

        return new PagedResult<PurchaseOrderDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
    {
        var order = await _context.PurchaseOrdersRepository
            .Include(p => p.Location)
            .Include(p => p.Items)
                .ThenInclude(i => i.LastName)
            .Include(p => p.Items)
                .ThenInclude(i => i.LastSize)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (order == null)
            return null;

        return new PurchaseOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            RequestedBy = order.RequestedBy,
            Department = order.Department,
            LocationId = order.LocationId,
            Status = order.Status,
            ReviewedBy = order.ReviewedBy,
            ReviewedAt = order.ReviewedAt,
            DenyReason = order.DenyReason,
            LocationName = order.Location.LocationName,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new PurchaseOrderItemDto
            {
                Id = i.Id,
                LastNameId = i.LastNameId,
                LastSizeId = i.LastSizeId,
                QuantityRequested = i.QuantityRequested,
                LastCode = i.LastName.LastCode,
                SizeLabel = i.LastSize.SizeLabel
            }).ToList()
        };
    }

    public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto dto, string requestedBy)
    {
        var orderNumber = GenerateOrderNumber();

        var order = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            RequestedBy = requestedBy,
            Department = dto.Department,
            LocationId = dto.LocationId,
            Status = "Pending"
        };

        _context.PurchaseOrdersRepository.Add(order);

        foreach (var itemDto in dto.Items)
        {
            var item = new PurchaseOrderItem
            {
                Id = Guid.NewGuid(),
                PurchaseOrderId = order.Id,
                LastNameId = itemDto.LastNameId,
                LastSizeId = itemDto.LastSizeId,
                QuantityRequested = itemDto.QuantityRequested
            };
            _context.PurchaseOrderItemsRepository.Add(item);
        }

        await _context.SaveChangesAsync();

        var location = await _context.LocationsRepository.FindAsync(dto.LocationId);
        return new PurchaseOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            RequestedBy = order.RequestedBy,
            Department = order.Department,
            LocationId = order.LocationId,
            Status = order.Status,
            LocationName = location?.LocationName,
            CreatedAt = order.CreatedAt,
            Items = dto.Items
        };
    }

    public async Task<bool> ConfirmOrderAsync(Guid id, string reviewedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = await _context.PurchaseOrdersRepository
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (order == null || order.Status != "Pending")
                return false;

            order.Status = "Confirmed";
            order.ReviewedBy = reviewedBy;
            order.ReviewedAt = DateTime.UtcNow;

            foreach (var item in order.Items)
            {
                var stock = await _context.InventoryStocksRepository
                    .FirstOrDefaultAsync(s => s.LastNameId == item.LastNameId
                        && s.LastSizeId == item.LastSizeId
                        && s.LocationId == order.LocationId);

                if (stock == null)
                {
                    stock = new InventoryStock
                    {
                        Id = Guid.NewGuid(),
                        LastNameId = item.LastNameId,
                        LastSizeId = item.LastSizeId,
                        LocationId = order.LocationId,
                        QuantityGood = item.QuantityRequested
                    };
                    _context.InventoryStocksRepository.Add(stock);
                }
                else
                {
                    stock.QuantityGood += item.QuantityRequested;
                }

                var movement = new StockMovement
                {
                    Id = Guid.NewGuid(),
                    LastNameId = item.LastNameId,
                    LastSizeId = item.LastSizeId,
                    ToLocationId = order.LocationId,
                    MovementType = "Purchase",
                    Quantity = item.QuantityRequested,
                    ReferenceNumber = order.OrderNumber,
                    CreatedBy = reviewedBy
                };
                _context.StockMovementsRepository.Add(movement);
            }

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

    public async Task<bool> DenyOrderAsync(Guid id, string reviewedBy, string reason)
    {
        var order = await _context.PurchaseOrdersRepository.FindAsync(id);
        if (order == null || order.Status != "Pending")
            return false;

        order.Status = "Denied";
        order.ReviewedBy = reviewedBy;
        order.ReviewedAt = DateTime.UtcNow;
        order.DenyReason = reason;

        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateOrderNumber()
    {
        var date = DateTime.UtcNow;
        var prefix = $"PO-{date:yyyyMMdd}";

        var lastOrder = _context.PurchaseOrdersRepository
            .Where(p => p.OrderNumber.StartsWith(prefix))
            .OrderByDescending(p => p.OrderNumber)
            .Select(p => p.OrderNumber)
            .FirstOrDefault();

        int sequence = 1;
        if (lastOrder != null)
        {
            var lastSeq = lastOrder.Split('-').Last();
            if (int.TryParse(lastSeq, out int parsed))
                sequence = parsed + 1;
        }

        return $"{prefix}-{sequence:D5}";
    }
}