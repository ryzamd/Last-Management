using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using LastManagement.Entities;
using LastManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class LocationService : ILocationService
{
    private readonly AppDbContext _context;

    public LocationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<LocationDto>> GetAllAsync(int page, int pageSize, string? locationType, bool? isActive)
    {
        var query = _context.LocationsRepository.AsQueryable();

        if (!string.IsNullOrEmpty(locationType))
            query = query.Where(l => l.LocationType == locationType);

        if (isActive.HasValue)
            query = query.Where(l => l.IsActive == isActive.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(l => l.LocationName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LocationDto
            {
                Id = l.Id,
                LocationCode = l.LocationCode,
                LocationName = l.LocationName,
                LocationType = l.LocationType,
                IsActive = l.IsActive,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<LocationDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<LocationDto?> GetByIdAsync(Guid id)
    {
        var location = await _context.LocationsRepository.FindAsync(id);
        if (location == null)
            return null;

        return new LocationDto
        {
            Id = location.Id,
            LocationCode = location.LocationCode,
            LocationName = location.LocationName,
            LocationType = location.LocationType,
            IsActive = location.IsActive,
            CreatedAt = location.CreatedAt
        };
    }

    public async Task<LocationDto> CreateAsync(LocationDto dto)
    {
        var location = new Location
        {
            Id = Guid.NewGuid(),
            LocationCode = dto.LocationCode,
            LocationName = dto.LocationName,
            LocationType = dto.LocationType,
            IsActive = dto.IsActive
        };

        _context.LocationsRepository.Add(location);
        await _context.SaveChangesAsync();

        dto.Id = location.Id;
        dto.CreatedAt = location.CreatedAt;
        return dto;
    }

    public async Task<LocationDto?> UpdateAsync(Guid id, LocationDto dto)
    {
        var location = await _context.LocationsRepository.FindAsync(id);
        if (location == null)
            return null;

        location.LocationCode = dto.LocationCode;
        location.LocationName = dto.LocationName;
        location.LocationType = dto.LocationType;
        location.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        dto.Id = location.Id;
        dto.CreatedAt = location.CreatedAt;
        return dto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var location = await _context.LocationsRepository.FindAsync(id);
        if (location == null)
            return false;

        _context.LocationsRepository.Remove(location);
        await _context.SaveChangesAsync();
        return true;
    }
}