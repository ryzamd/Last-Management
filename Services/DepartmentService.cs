using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.Entities;
using LastManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;

    public DepartmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        return await _context.DepartmentsRepository
            .Where(d => d.IsActive)
            .OrderBy(d => d.DepartmentName)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        var dept = await _context.DepartmentsRepository.FindAsync(id);
        if (dept == null)
            return null;

        return new DepartmentDto
        {
            Id = dept.Id,
            DepartmentName = dept.DepartmentName,
            IsActive = dept.IsActive,
            CreatedAt = dept.CreatedAt
        };
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
    {
        var exists = await _context.DepartmentsRepository.AnyAsync(d => d.DepartmentName == dto.DepartmentName);
        if (exists)
            throw new InvalidOperationException($"Department with name '{dto.DepartmentName}' already exists");

        var dept = new Department
        {
            Id = Guid.NewGuid(),
            DepartmentName = dto.DepartmentName,
            IsActive = dto.IsActive
        };

        _context.DepartmentsRepository.Add(dept);
        await _context.SaveChangesAsync();

        dto.Id = dept.Id;
        dto.CreatedAt = dept.CreatedAt;
        return dto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var dept = await _context.DepartmentsRepository.FindAsync(id);
        if (dept == null)
            return false;

        _context.DepartmentsRepository.Remove(dept);
        await _context.SaveChangesAsync();
        return true;
    }
}