using LastManagement.DTOs;
using LastManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LastManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LastSizesController : ControllerBase
{
    private readonly ILastSizeService _lastSizeService;

    public LastSizesController(ILastSizeService lastSizeService)
    {
        _lastSizeService = lastSizeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _lastSizeService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _lastSizeService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] LastSizeDto dto)
    {
        var result = await _lastSizeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LastSizeDto dto)
    {
        var result = await _lastSizeService.UpdateAsync(id, dto);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _lastSizeService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}