using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.Entities;
using LastManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class LastSizeService : ILastSizeService
{
    private readonly AppDbContext _context;

    public LastSizeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LastSizeDto>> GetAllAsync()
    {
        return await _context.LastSizesRepository
            .OrderBy(s => s.SizeValue)
            .Select(s => new LastSizeDto
            {
                Id = s.Id,
                SizeValue = s.SizeValue,
                SizeLabel = s.SizeLabel,
                Status = s.Status,
                ReplacementSizeId = s.ReplacementSizeId,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<LastSizeDto?> GetByIdAsync(Guid id)
    {
        var size = await _context.LastSizesRepository.FindAsync(id);
        if (size == null)
            return null;

        return new LastSizeDto
        {
            Id = size.Id,
            SizeValue = size.SizeValue,
            SizeLabel = size.SizeLabel,
            Status = size.Status,
            ReplacementSizeId = size.ReplacementSizeId,
            CreatedAt = size.CreatedAt
        };
    }

    public async Task<LastSizeDto> CreateAsync(LastSizeDto dto)
    {
        var exists = await _context.LastSizesRepository.AnyAsync(s => s.SizeValue == dto.SizeValue);
        if (exists)
            throw new InvalidOperationException($"Size with value '{dto.SizeValue}' already exists");

        if (dto.ReplacementSizeId.HasValue)
        {
            var replacementExists = await _context.LastSizesRepository.AnyAsync(s => s.Id == dto.ReplacementSizeId.Value);
            if (!replacementExists)
                throw new ArgumentException($"Replacement size with ID '{dto.ReplacementSizeId}' not found");
        }

        var size = new LastSize
        {
            Id = Guid.NewGuid(),
            SizeValue = dto.SizeValue,
            SizeLabel = dto.SizeLabel,
            Status = dto.Status,
            ReplacementSizeId = dto.ReplacementSizeId
        };

        _context.LastSizesRepository.Add(size);
        await _context.SaveChangesAsync();

        dto.Id = size.Id;
        dto.CreatedAt = size.CreatedAt;
        return dto;
    }

    public async Task<LastSizeDto?> UpdateAsync(Guid id, LastSizeDto dto)
    {
        var size = await _context.LastSizesRepository.FindAsync(id);
        if (size == null)
            return null;

        var exists = await _context.LastSizesRepository.AnyAsync(s => s.SizeValue == dto.SizeValue && s.Id != id);
        if (exists)
            throw new InvalidOperationException($"Size with value '{dto.SizeValue}' already exists");

        if (dto.ReplacementSizeId.HasValue)
        {
            var replacementExists = await _context.LastSizesRepository.AnyAsync(s => s.Id == dto.ReplacementSizeId.Value);
            if (!replacementExists)
                throw new ArgumentException($"Replacement size with ID '{dto.ReplacementSizeId}' not found");
        }

        size.SizeValue = dto.SizeValue;
        size.SizeLabel = dto.SizeLabel;
        size.Status = dto.Status;
        size.ReplacementSizeId = dto.ReplacementSizeId;

        await _context.SaveChangesAsync();

        dto.Id = size.Id;
        dto.CreatedAt = size.CreatedAt;
        return dto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var size = await _context.LastSizesRepository.FindAsync(id);
        if (size == null)
            return false;

        var hasStocks = await _context.InventoryStocksRepository.AnyAsync(i => i.LastSizeId == id);
        if (hasStocks)
            throw new InvalidOperationException("Cannot delete size with existing inventory stocks");

        var hasPurchaseOrderItems = await _context.PurchaseOrderItemsRepository.AnyAsync(p => p.LastSizeId == id);
        if (hasPurchaseOrderItems)
            throw new InvalidOperationException("Cannot delete size with existing purchase order items");

        var isReplacementSize = await _context.LastSizesRepository.AnyAsync(s => s.ReplacementSizeId == id);
        if (isReplacementSize)
            throw new InvalidOperationException("Cannot delete size that is used as replacement for other sizes");

        _context.LastSizesRepository.Remove(size);
        await _context.SaveChangesAsync();
        return true;
    }
}