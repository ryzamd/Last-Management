using LastManagement.Database;
using LastManagement.DTOs;
using LastManagement.DTOs.Shared;
using LastManagement.Entities;
using LastManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastManagement.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<CustomerDto>> GetAllAsync(int page, int pageSize, string? status)
    {
        var query = _context.CustomersRepository.AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(c => c.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.CustomerName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                CustomerName = c.CustomerName,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return new PagedResult<CustomerDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _context.CustomersRepository.FindAsync(id);
        if (customer == null)
            return null;

        return new CustomerDto
        {
            Id = customer.Id,
            CustomerName = customer.CustomerName,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public async Task<CustomerDto> CreateAsync(CustomerDto dto)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            CustomerName = dto.CustomerName,
            Status = dto.Status
        };

        _context.CustomersRepository.Add(customer);
        await _context.SaveChangesAsync();

        dto.Id = customer.Id;
        dto.CreatedAt = customer.CreatedAt;
        dto.UpdatedAt = customer.UpdatedAt;
        return dto;
    }

    public async Task<CustomerDto?> UpdateAsync(Guid id, CustomerDto dto)
    {
        var customer = await _context.CustomersRepository.FindAsync(id);
        if (customer == null)
            return null;

        customer.CustomerName = dto.CustomerName;
        customer.Status = dto.Status;

        await _context.SaveChangesAsync();

        dto.Id = customer.Id;
        dto.CreatedAt = customer.CreatedAt;
        dto.UpdatedAt = customer.UpdatedAt;
        return dto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _context.CustomersRepository.FindAsync(id);
        if (customer == null)
            return false;

        _context.CustomersRepository.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}