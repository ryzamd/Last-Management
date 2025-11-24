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

        _context.LastSizesRepository.Remove(size);
        await _context.SaveChangesAsync();
        return true;
    }
}