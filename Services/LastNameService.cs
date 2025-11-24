using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using LastManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class LastNameService : ILastNameService
{
    private readonly AppDbContext _context;

    public LastNameService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<LastNameDto>> GetAllAsync(int page, int pageSize, Guid? customerId, string? status)
    {
        var query = _context.LastNamesRepository.Include(l => l.Customer).AsQueryable();

        if (customerId.HasValue)
            query = query.Where(l => l.CustomerId == customerId.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(l => l.LastStatus == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(l => l.LastCode)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LastNameDto
            {
                Id = l.Id,
                LastCode = l.LastCode,
                LastType = l.LastType,
                Article = l.Article,
                LastStatus = l.LastStatus,
                CustomerId = l.CustomerId,
                CustomerName = l.Customer.CustomerName,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<LastNameDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<LastNameDto?> GetByIdAsync(Guid id)
    {
        var last = await _context.LastNamesRepository.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        if (last == null)
            return null;

        return new LastNameDto
        {
            Id = last.Id,
            LastCode = last.LastCode,
            LastType = last.LastType,
            Article = last.Article,
            LastStatus = last.LastStatus,
            CustomerId = last.CustomerId,
            CustomerName = last.Customer.CustomerName,
            CreatedAt = last.CreatedAt
        };
    }

    public async Task<LastNameDto> CreateAsync(LastNameDto dto)
    {
        var last = new LastName
        {
            Id = Guid.NewGuid(),
            LastCode = dto.LastCode,
            LastType = dto.LastType,
            Article = dto.Article,
            LastStatus = dto.LastStatus,
            CustomerId = dto.CustomerId
        };

        _context.LastNamesRepository.Add(last);
        await _context.SaveChangesAsync();

        var customer = await _context.CustomersRepository.FindAsync(dto.CustomerId);
        return new LastNameDto
        {
            Id = last.Id,
            LastCode = last.LastCode,
            LastType = last.LastType,
            Article = last.Article,
            LastStatus = last.LastStatus,
            CustomerId = last.CustomerId,
            CustomerName = customer?.CustomerName,
            CreatedAt = last.CreatedAt
        };
    }

    public async Task<LastNameDto?> UpdateAsync(Guid id, LastNameDto dto)
    {
        var last = await _context.LastNamesRepository.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        if (last == null)
            return null;

        last.LastCode = dto.LastCode;
        last.LastType = dto.LastType;
        last.Article = dto.Article;
        last.LastStatus = dto.LastStatus;
        last.CustomerId = dto.CustomerId;

        await _context.SaveChangesAsync();

        return new LastNameDto
        {
            Id = last.Id,
            LastCode = last.LastCode,
            LastType = last.LastType,
            Article = last.Article,
            LastStatus = last.LastStatus,
            CustomerId = last.CustomerId,
            CustomerName = last.Customer.CustomerName,
            CreatedAt = last.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var last = await _context.LastNamesRepository.FindAsync(id);
        if (last == null)
            return false;

        _context.LastNamesRepository.Remove(last);
        await _context.SaveChangesAsync();
        return true;
    }
}